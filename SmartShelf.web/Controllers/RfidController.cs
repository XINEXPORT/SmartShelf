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
            return BadRequest(ex.Message);
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
                .Select(t => t.EPC)
                .ToHashSet();

            var tagReadEvents = new List<TagReadEvent>();

            foreach (var tag in tags)
            {
                if (!validEpcs.Contains(tag.EPC))
                    continue;

                tagReadEvents.Add(new TagReadEvent
                {
                    EPC = tag.EPC,
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

            service.Disconnect();

            return Ok(tagReadEvents);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}