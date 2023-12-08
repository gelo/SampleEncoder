using log4net.Repository.Hierarchy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SampleEncoder.Api.Models;

[ApiController]
[Route("[controller]")]
public class Base64Controller : ControllerBase
{
    private readonly IBase64Service base64Service;
    private readonly ILogger<Base64Controller> logger;

    public Base64Controller(IBase64Service base64Service, ILogger<Base64Controller> logger)
    {
        this.base64Service = base64Service;
        this.logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<string>> EncodeToBase64([FromBody] TextEncodeRequest request)
    {
        try
        {
            string convertedInputText = await base64Service.EncodeToBase64Async(request.Text);
            return Ok(new { Base64Result = convertedInputText });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest($"Error during encoding: {ex.Message}");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error during encoding");
            return StatusCode(500, "Unexpected error during encoding. Please try again later.");
        }
    }

    [HttpGet("stream")]
    public async Task<IActionResult> GetBase64Stream()
    {
        try
        {
            Response.ContentType = "text/event-stream";

            var responseItems = await base64Service.GetBase64StreamAsync();

            foreach (var item in responseItems)
            {
                var jsonProgress = JsonConvert.SerializeObject(item);
                await Response.WriteAsync($"data: {jsonProgress}\n\n");
                await Response.Body.FlushAsync();
                await Task.Delay(4000);
            }

            return Ok(); 
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error streaming");
            return StatusCode(500, "Unexpected error during streaming. Please try again later.");
        }
    }

    [HttpPost("cancel")]
    public IActionResult CancelEncoding()
    {
        try
        {
            base64Service.CancelEncoding();
            return Ok("Encoding process canceled.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error canceling encoding");
            return StatusCode(500, "Error canceling encoding. Please try again later.");
        }
    }
}
