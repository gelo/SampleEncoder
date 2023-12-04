using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using SampleEncoder.Api.Models;
using Newtonsoft.Json;

[ApiController]
[Route("[controller]")]
public class Base64Controller : ControllerBase
{
    private readonly IBase64Service base64Service;

    public Base64Controller(IBase64Service base64Service)
    {
        this.base64Service = base64Service;
    }

    [HttpPost]
    public async Task<IActionResult> EncodeToBase64([FromBody] TextEncodeRequest request)
    {
        try
        {
            string convertedInputText = await base64Service.EncodeToBase64Async(request.Text);
            return Ok(new { Base64Result = convertedInputText });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("stream")]
    public async Task GetBase64Stream()
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
    }

    [HttpPost("cancel")]
    public IActionResult CancelEncoding()
    {
        base64Service.CancelEncoding();
        return Ok("Encoding process canceled.");
    }
}
