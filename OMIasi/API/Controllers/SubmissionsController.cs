using System.IdentityModel.Tokens.Jwt;
using Application.Features.Submissions.Commands.Create;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Application.Features.Submissions.Queries.GetAll;

namespace API.Controllers;

[Route("submissions")]
public class SubmissionsController: ApiControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAll(Guid id)
    {
        var userId = User.FindFirstValue(JwtRegisteredClaimNames.Jti);
        if (userId == null)
        {
            return Unauthorized();
        }

        var result = await Mediator.Send(new GetAllSubmissionsByProblemIdAndUserIdQuery()
        {
            UserId = Guid.Parse(userId),
            ProblemId = id
        });

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSubmissionCommand command)
    {
        var userId = User.FindFirstValue(JwtRegisteredClaimNames.Jti);
        if (userId == null)
        {
            return Unauthorized();
        }
        command.UserId = Guid.Parse(userId);
        var result = await Mediator.Send(command);
        return Ok(result.Results);
    }
}