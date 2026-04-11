using Microsoft.AspNetCore.Mvc;
using SmartShelf.web.Models;
using SmartShelf.web.Data;
using System.Collections.Generic;
using System.Linq;



[ApiController]
[Route("api/rfid")]
public class RfidController : ControllerBase
{
    [HttpGet("read-tags")]
    public IActionResult ReadTags()
    {
        var service = new RfidReaderService();

        try
        {
            service.Connect("tmr:///COM4");

            var tags = service.ReadTags(1000);

            service.Disconnect();

            return Ok(tags);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.InnerException?.Message ?? ex.Message);
        }
        finally {
            service.Disconnect();
        }
    }

    private readonly SmartShelfContext _context;

    public RfidController(SmartShelfContext context)
    {
        _context = context;
    }

    [HttpPost("read-and-save")]
    public IActionResult ReadAndSave()
    {
        var service = new RfidReaderService();

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

            // Save read history
            _context.TagReadEvent.AddRange(tagReadEvents);
            _context.SaveChanges();

            // Get the most recent read per EPC for current state updates
            var latestReadsPerTag = tagReadEvents
                .GroupBy(t => t.EPC)
                .Select(g => g.OrderByDescending(x => x.Timestamp).First())
                .ToList();

            foreach (var latestRead in latestReadsPerTag)
            {
                var existingState = _context.TagCurrentState
                    .FirstOrDefault(tcs => tcs.EPC == latestRead.EPC);

                if (existingState == null)
                {
                    _context.TagCurrentState.Add(new TagCurrentState
                    {
                        EPC = latestRead.EPC,
                        ReaderId = latestRead.ReaderId,
                        Antenna = latestRead.Antenna,
                        Rssi = latestRead.Rssi,
                        LastSeenTimestamp = latestRead.Timestamp,
                        ReadCount = latestRead.ReadCount,
                        Frequency = latestRead.Frequency
                    });
                }
                else
                {
                    existingState.ReaderId = latestRead.ReaderId;
                    existingState.Antenna = latestRead.Antenna;
                    existingState.Rssi = latestRead.Rssi;
                    existingState.LastSeenTimestamp = latestRead.Timestamp;
                    existingState.ReadCount = latestRead.ReadCount;
                    existingState.Frequency = latestRead.Frequency;
                }
            }

            _context.SaveChanges();

            return Ok(new
            {
                message = "Tags saved successfully.",
                totalTagsRead = tags.Count,
                savedCount = tagReadEvents.Count,
                skippedCount = skippedEpcs.Count,
                skippedEpcs = skippedEpcs.Distinct().ToList(),
                currentStateUpdated = latestReadsPerTag.Count
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
                ProductId = tcs.Tag.ProductId,
                ProductName = tcs.Tag.Product.Name
            })
            .OrderByDescending(x => x.LastSeenTimestamp)
            .ToList();

        return Ok(currentState);
    }
}

