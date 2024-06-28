using Application.Features.Users.Queries.Commands.ChangeRole;
using Application.Features.Users.Queries.Commands.Delete;
using Application.Features.Users.Queries.GetProfile;
using Domain.Data;
using Microsoft.AspNetCore.Authorization;
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

    [HttpPut("changerole")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeRole([FromBody] ChangeRoleCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("username")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeRole(string username)
    {
        var result = await Mediator.Send(new DeleteUserCommand(username));
        return Ok(result);
    }
}