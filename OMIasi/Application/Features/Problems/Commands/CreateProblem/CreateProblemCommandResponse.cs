using Application.Responses;

namespace Application.Features.Problems.Commands.CreateProblem;

public class CreateProblemCommandResponse : BaseResponse
{
    public ProblemDto Problem { get; set; } = null!;
}