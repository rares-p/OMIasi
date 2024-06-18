using Application.Contracts.Repositories;
using MediatR;

namespace Application.Features.Submissions.Queries.GetAll;

public class GetAllSubmissionsByProblemIdAndUserIdQueryHandler(ISubmissionRepository submissionRepository) : IRequestHandler<GetAllSubmissionsByProblemIdAndUserIdQuery, GetAllSubmissionsByProblemIdAndUserIdResponse>
{
    public async Task<GetAllSubmissionsByProblemIdAndUserIdResponse> Handle(GetAllSubmissionsByProblemIdAndUserIdQuery request, CancellationToken cancellationToken)
    {
        var submissions =
            await submissionRepository.GetSubmissionsByProblemIdAndUserId(request.ProblemId, request.UserId);

        return new GetAllSubmissionsByProblemIdAndUserIdResponse()
        {
            Success = true,
            Submissions = submissions.Value.Select(s => new SubmissionDto()
            {
                Id = s.Id,
                Date = s.Date,
                Solution = s.Solution,
                Scores = s.Scores.Select(score => new SubmissionTestDto()
                {
                    Id = score.Id,
                    Message = score.Message,
                    Score = score.Score,
                    Runtime = score.Runtime,
                    TestIndex = score.TestIndex
                }).OrderBy(submissionTestDto => submissionTestDto.TestIndex).ToList()
            })
        };
    }
}