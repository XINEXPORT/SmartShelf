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

    public RfidController(SmartShelfContext context)
    {
        _context = context;
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

    [HttpPost("read-and-save")]
    public IActionResult ReadAndSave()
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

            // Save tag read history
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

            // Group current scan reads by EPC
            var readsGroupedByEpc = tagReadEvents
                .GroupBy(t => t.EPC)
                .ToDictionary(
                    g => g.Key,
                    g => g.OrderByDescending(x => x.Timestamp).ToList()
                );

            var seenEpcs = readsGroupedByEpc.Keys.ToHashSet();

            // Update current state for tags seen in this scan
            foreach (var kvp in readsGroupedByEpc)
            {
                string epc = kvp.Key;
                List<TagReadEvent> readsForTag = kvp.Value;

                var latest = readsForTag.First();
                var strongest = readsForTag.OrderByDescending(x => x.Rssi).First(); //only add the strongest antenna read, so that will be the localization
                int totalReadCount = readsForTag.Sum(x => x.ReadCount);
                bool isPresent = presenceService.IsTagPresent(readsForTag);

                var existingState = _context.TagCurrentState
                    .FirstOrDefault(tcs => tcs.EPC == epc);

                if (existingState == null)
                {
                    _context.TagCurrentState.Add(new TagCurrentState
                    {
                        EPC = epc,
                        ReaderId = latest.ReaderId,
                        Antenna = strongest.Antenna,
                        Rssi = strongest.Rssi,
                        LastSeenTimestamp = latest.Timestamp,
                        ReadCount = totalReadCount,
                        Frequency = strongest.Frequency,
                        IsPresent = isPresent,
                        MissedScanCount = 0
                    });
                }
                else
                {
                    existingState.ReaderId = latest.ReaderId;
                    existingState.Antenna = strongest.Antenna;
                    existingState.Rssi = strongest.Rssi;
                    existingState.LastSeenTimestamp = latest.Timestamp;
                    existingState.ReadCount = totalReadCount;
                    existingState.Frequency = strongest.Frequency;

                    // If the tag is seen at all, reset misses
                    existingState.MissedScanCount = 0;

                    // If this scan is strong enough, mark present
                    if (isPresent)
                    {
                        existingState.IsPresent = true;
                    }
                }
            }

            // Mark tags not seen in this scan as not present
            var unseenStates = _context.TagCurrentState
                .Where(tcs => !seenEpcs.Contains(tcs.EPC))
                .ToList();

            foreach (var state in unseenStates)
            {
                // Keep count of missed scans
                state.MissedScanCount++;

                if (state.MissedScanCount >= 3)
                {
                    state.IsPresent = false;
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
