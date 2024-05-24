using Application.Features.Problems.Commands.CreateProblem;
using Application.Features.Problems.Queries.GetAll;
using Application.Features.Problems.Queries.GetById;
using Application.Features.Problems.Queries.GetByName;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("problems")]
public class ProblemController: ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await Mediator.Send(new GetAllProblemsQuery());

        return Ok(result.Problems);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await Mediator.Send(new GetByIdProblemQuery(id));

        return Ok(result);
    }

    [HttpGet("{title}")]
    public async Task<IActionResult> GetById(string title)
    {
        var result = await Mediator.Send(new GetByTitleProblemQuery(title));

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProblemCommand command)
    {
        var result = await Mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
}