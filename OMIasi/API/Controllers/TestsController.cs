using Application.Features.Tests.Queries.GetById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("tests")]
public class TestsController : ApiControllerBase
{
    [HttpGet]
    //[Authorize(Roles = "Admin")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAllByProblemId(Guid id)
    {
        var result = await Mediator.Send(new GetTestByIdQuery(id));
        return Ok(result);
    }
}