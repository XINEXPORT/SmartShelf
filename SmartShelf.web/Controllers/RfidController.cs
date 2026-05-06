using Microsoft.AspNetCore.Mvc;
using SmartShelf.web.Models;
using SmartShelf.web.Data;
using SmartShelf.web.Services;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("api/rfid")]
public class RfidController : ControllerBase
{
    private readonly SmartShelfContext _context;
    private readonly EmailService _emailService;

    public RfidController(SmartShelfContext context, EmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    [HttpGet("read-tags")]
    public IActionResult ReadTags()
    {
        var service = new RfidReaderService();

        try
        {
            service.Connect("tmr:///COM4");
            var tags = service.ReadTags(1000);
            return Ok(tags);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.InnerException?.Message ?? ex.Message);
        }
        finally
        {
            service.Disconnect();
        }
    }

    [HttpPost("test-low-stock-email")]
    public async Task<IActionResult> TestLowStockEmail()
    {
        try
        {
            var productStock = _context.Product
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Threshold,
                    CurrentStock = _context.TagCurrentState.Count(tcs =>
                        tcs.IsPresent &&
                        tcs.Tag.ProductId == p.Id)
                })
                .ToList();

            var lowStockProducts = productStock
                .Where(p => p.CurrentStock <= p.Threshold)
                .ToList();

            if (lowStockProducts.Count == 0)
            {
                return Ok(new
                {
                    message = "No low stock products found.",
                    productStock
                });
            }

            foreach (var product in lowStockProducts)
            {
                await _emailService.SendLowStockEmailAsync(
                    product.Name,
                    product.CurrentStock,
                    product.Threshold
                );
            }

            return Ok(new
            {
                message = "Low stock email test completed.",
                lowStockProducts
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.InnerException?.Message ?? ex.Message);
        }
    }

    [HttpPost("read-and-save")]
    public async Task<IActionResult> ReadAndSave()
    {
        var service = new RfidReaderService();
        var presenceService = new TagPresenceService();

        try
        {
            service.Connect("tmr:///COM4");

            var tags = service.ReadTags(3000);

            var validEpcs = _context.Tag
                .Select(t => t.EPC.Trim().ToUpper())
                .ToHashSet();

            var tagReadEvents = new List<TagReadEvent>();
            var skippedEpcs = new List<string>();

            foreach (var tag in tags)
            {
                var epc = tag.EPC.Trim().ToUpper();

                if (!validEpcs.Contains(epc))
                {
                    skippedEpcs.Add(epc);
                    continue;
                }

                tagReadEvents.Add(new TagReadEvent
                {
                    EPC = epc,
                    ReaderId = 1,
                    Timestamp = tag.Timestamp,
                    Antenna = tag.Antenna,
                    Rssi = tag.Rssi,
                    ReadCount = tag.ReadCount,
                    Frequency = tag.Frequency
                });
            }

            if (tagReadEvents.Count == 0)
            {
                return Ok(new
                {
                    message = "No known tags were saved. All scanned tags were skipped because they are not in the Tag table.",
                    totalTagsRead = tags.Count,
                    savedCount = 0,
                    skippedCount = skippedEpcs.Count,
                    skippedEpcs = skippedEpcs.Distinct().ToList(),
                    currentStateUpdated = 0,
                    presentCount = _context.TagCurrentState.Count(t => t.IsPresent),
                    absentCount = _context.TagCurrentState.Count(t => !t.IsPresent)
                });
            }

            _context.TagReadEvent.AddRange(tagReadEvents);
            _context.SaveChanges();

            var readsGroupedByEpc = tagReadEvents
                .GroupBy(t => t.EPC)
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderByDescending(x => x.Timestamp).ToList()
                );

            var seenEpcs = readsGroupedByEpc.Keys.ToHashSet();

            foreach (var kvp in readsGroupedByEpc)
            {
                string epc = kvp.Key;
                List<TagReadEvent> readsForTag = kvp.Value;

                var latest = readsForTag.First();
                bool isPresent = presenceService.IsTagPresent(readsForTag);

                var existingState = _context.TagCurrentState
                    .FirstOrDefault(tcs => tcs.EPC == epc);

                if (existingState == null)
                {
                    _context.TagCurrentState.Add(new TagCurrentState
                    {
                        EPC = epc,
                        ReaderId = latest.ReaderId,
                        Antenna = latest.Antenna,
                        Rssi = latest.Rssi,
                        LastSeenTimestamp = latest.Timestamp,
                        ReadCount = latest.ReadCount,
                        Frequency = latest.Frequency,
                        IsPresent = isPresent,
                        MissedScanCount = 0
                    });
                }
                else
                {
                    existingState.ReaderId = latest.ReaderId;
                    existingState.Antenna = latest.Antenna;
                    existingState.Rssi = latest.Rssi;
                    existingState.LastSeenTimestamp = latest.Timestamp;
                    existingState.ReadCount = latest.ReadCount;
                    existingState.Frequency = latest.Frequency;
                    existingState.MissedScanCount = 0;

                    if (isPresent)
                    {
                        existingState.IsPresent = true;
                    }
                }
            }

            var unseenStates = _context.TagCurrentState
                .Where(tcs => !seenEpcs.Contains(tcs.EPC))
                .ToList();

            foreach (var state in unseenStates)
            {
                state.MissedScanCount++;

                if (state.MissedScanCount >= 3)
                {
                    state.IsPresent = false;
                }
            }

            _context.SaveChanges();

            //track isLowStockAlertActive
            //to determine when to send an email notification
            var products = _context.Product.ToList();

            foreach (var product in products)
            {
                var currentStock = _context.TagCurrentState.Count(tcs =>
                    tcs.IsPresent &&
                    tcs.Tag.ProductId == product.Id);

                bool isLowStockNow = currentStock <= product.Threshold;

                if (isLowStockNow && !product.IsLowStockAlertActive)
                {
                    await _emailService.SendLowStockEmailAsync(
                        product.Name,
                        currentStock,
                        product.Threshold
                    );

                    product.IsLowStockAlertActive = true;
                }
                else if (!isLowStockNow && product.IsLowStockAlertActive)
                {
                    product.IsLowStockAlertActive = false;
                }
            }

            _context.SaveChanges();

            return Ok(new
            {
                message = "Tags processed successfully.",
                totalTagsRead = tags.Count,
                savedCount = tagReadEvents.Count,
                skippedCount = skippedEpcs.Count,
                skippedEpcs = skippedEpcs.Distinct().ToList(),
                currentStateUpdated = readsGroupedByEpc.Count,
                presentCount = _context.TagCurrentState.Count(t => t.IsPresent),
                absentCount = _context.TagCurrentState.Count(t => !t.IsPresent)
            });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.InnerException?.Message ?? ex.Message);
        }
        finally
        {
            service.Disconnect();
        }
    }

    [HttpGet("current-state")]
    public IActionResult GetCurrentState()
    {
        var currentState = _context.TagCurrentState
            .Select(tcs => new
            {
                tcs.EPC,
                tcs.ReaderId,
                ReaderLocation = tcs.Reader.Location,
                tcs.Antenna,
                tcs.Rssi,
                tcs.LastSeenTimestamp,
                tcs.ReadCount,
                tcs.Frequency,
                tcs.IsPresent,
                tcs.MissedScanCount,
                ProductId = tcs.Tag.ProductId,
                ProductName = tcs.Tag.Product.Name
            })
            .OrderByDescending(x => x.LastSeenTimestamp)
            .ToList();

        return Ok(currentState);
    }
}