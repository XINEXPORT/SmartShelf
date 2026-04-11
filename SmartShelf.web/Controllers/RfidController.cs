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

    /*
    Add the DbContext field and constructor injection
    */
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
                    skippedEpcs.Add(tag.EPC);
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

            _context.TagReadEvent.AddRange(tagReadEvents);
            _context.SaveChanges();

            return Ok(new
            {
                message = "Tags saved successfully.",
                totalTagsRead = tags.Count,
                savedCount = tagReadEvents.Count,
                skippedCount = skippedEpcs.Count,
                skippedEpcs = skippedEpcs.Distinct().ToList()
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
}

