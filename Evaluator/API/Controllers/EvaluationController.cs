using Application.Contracts;
using Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("evaluate")]
public class EvaluationController(IEvaluationService evaluationService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Evaluate([FromBody] Evaluation evaluation)
    {
        var result = await evaluationService.Evaluate(evaluation);
        return Ok(result);
    }
}