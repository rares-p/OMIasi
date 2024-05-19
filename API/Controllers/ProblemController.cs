using Application.Features.Problems.Commands.CreateProblem;
using Application.Features.Problems.Queries.GetAll;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("problems")]
public class ProblemController: ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await Mediator.Send(new GetAllProblemsQuery());

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProblemCommand command)
    {
        var result = await Mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}