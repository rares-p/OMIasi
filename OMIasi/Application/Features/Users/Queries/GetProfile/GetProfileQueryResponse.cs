using Application.Responses;

namespace Application.Features.Users.Queries.GetProfile;

public class GetProfileQueryResponse : BaseResponse
{
    public string Username { get; set; }
    public string Role { get; set; }
    public List<string> SolvedProblems { get; set; }
    public List<string> AttemptedProblems { get; set; }
}