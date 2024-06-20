using Application.Features.Users.Queries.GetProfile;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("users")]
public class UserController : ApiControllerBase
{
    [HttpGet("username")]
    public async Task<IActionResult> GetUserProfile(string username)
    {
        var result = await Mediator.Send(new GetProfileQuery(username));
        return Ok(result);
    }
}