using Application.Contracts.Repositories;
using MediatR;

namespace Application.Features.Problems.Commands.Delete;

public class DeleteProblemCommandHandler(IProblemRepository problemRepository) : IRequestHandler<DeleteProblemCommand, DeleteProblemCommandResponse>
{
    public async Task<DeleteProblemCommandResponse> Handle(DeleteProblemCommand request, CancellationToken cancellationToken)
    {
        var result = await problemRepository.DeleteAsync(request.ProblemId);
        if (!result.IsSuccess)
            return new DeleteProblemCommandResponse
            {
                Success = false,
                Error = result.Error
            };

        return new DeleteProblemCommandResponse
        {
            Success = true
        };
    }
}