using Application.Responses;

namespace Application.Features.Submissions.Queries.GetAll;

public class GetAllSubmissionsByProblemIdAndUserIdResponse : BaseResponse
{
    public IEnumerable<SubmissionDto> Submissions { get; set; }
}