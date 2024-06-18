using Application.Contracts;
using Application.Contracts.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.Submissions.Commands.Create;

public class CreateSubmissionCommandHandler(IProblemRepository problemRepository, IEvaluationService evaluationService, ISubmissionRepository submissionRepository) : IRequestHandler<CreateSubmissionCommand, CreateSubmissionCommandResponse>
{
    public async Task<CreateSubmissionCommandResponse> Handle(CreateSubmissionCommand request, CancellationToken cancellationToken)
    {
        var problemResult = await problemRepository.FindByIdAsync(request.ProblemId);
        if (!problemResult.IsSuccess)
            return new CreateSubmissionCommandResponse()
            {
                Error = problemResult.Error,
                Success = false
            };
        var evaluationResponse = await evaluationService.Evaluate(request.ProblemId, request.Solution);
        if (!evaluationResponse.IsSuccess)
            return new CreateSubmissionCommandResponse()
            {
                Error = evaluationResponse.Error,
                Success = false
            };
        try
        {
            var testResults = evaluationResponse.Value.Select(resp => (resp.Message, resp.Score, resp.Runtime, resp.TestIndex)).ToList();
            var submission = Submission.Create(request.UserId, request.ProblemId, request.Solution,
                (uint)evaluationResponse.Value.Sum(test => test.Score), DateTime.Now, testResults);
            if (!submission.IsSuccess)
                return new CreateSubmissionCommandResponse()
                {
                    Success = false,
                    Error = submission.Error
                };
            await submissionRepository.AddAsync(submission.Value);
        }
        catch (Exception e)
        {
            return new CreateSubmissionCommandResponse()
            {
                Error = "Server error, Please try again later",
                Success = false
            };
        }
        return new CreateSubmissionCommandResponse()
        {
            Success = true,
            Results = [..evaluationResponse.Value]
        };
    }
}