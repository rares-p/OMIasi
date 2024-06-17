using MediatR;

namespace Application.Features.Submissions.Queries.GetAll;

public class GetAllSubmissionsByProblemIdAndUserIdQuery : IRequest<GetAllSubmissionsByProblemIdAndUserIdResponse>
{
    public Guid UserId { get; init; }
    public Guid ProblemId { get; init; }
}