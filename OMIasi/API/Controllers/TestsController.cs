using Application.Features.Tests.Queries.GetAll;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("tests")]
public class TestsController : ApiControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [HttpGet("{problemId:guid}")]
    public async Task<IActionResult> GetAllByProblemId(Guid problemId)
    {
        var result = await Mediator.Send(new GetAllTestsQuery(problemId));
        return Ok(result);
    }
}