using Application.Features.Problems.Commands.CreateProblem;
using Application.Features.Problems.Commands.Delete;
using Application.Features.Problems.Commands.Update;
using Application.Features.Problems.Queries.GetAll;
using Application.Features.Problems.Queries.GetById;
using Application.Features.Problems.Queries.GetByName;
using Application.Features.Problems.Queries.GetProblemAndTestsById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("problems")]
public class ProblemController : ApiControllerBase
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

    [HttpGet("admin/{id:guid}")]
    public async Task<IActionResult> GetProblemAndTestsById(Guid id)
    {
        var result = await Mediator.Send(new GetProblemAndTestsByIdQuery(id));
        if (!result.IsSuccess)
            return BadRequest(result.Error);

        return Ok(result.Value);
    }

    [HttpPost]
    [Authorize(Roles = "Teacher,Admin")]
    public async Task<IActionResult> Create([FromBody] CreateProblemCommand command)
    {
        var result = await Mediator.Send(command);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateProblemCommand command)
    {
        var result = await Mediator.Send(command);

        //if (!result.Success)
        //    return BadRequest(result);

        return Ok(result);
    }


    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Teacher,Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await Mediator.Send(new DeleteProblemCommand(id));

        if (result.Success)
            return Ok(result);
        return BadRequest(result);
    }
}