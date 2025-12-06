using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace UnoBookRail.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class QuickReportsController : ControllerBase
{
    [HttpPost("Create")]
    public async Task<ActionResult<string>> CreateReport(
        [FromForm] IFormFile imageFile, [FromForm] string location, [FromForm] string information)
    {
        // A real app would do something with the provided data
        Debug.WriteLine($"{imageFile} {location} {information}");

        return await Task.FromResult<ActionResult<string>>("success");
    }
}