using Microsoft.AspNetCore.Mvc;

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
}