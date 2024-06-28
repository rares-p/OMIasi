using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("")]
public class StatusController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetStatus()
    {
        return Ok();
    }
}