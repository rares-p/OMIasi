using Application.Models;
using Application.Responses;

namespace Application.Features.Submissions.Commands.Create;

public class CreateSubmissionCommandResponse : BaseResponse
{
    public Guid Id { get; set; }
    public List<TestResultModel> Results { get; set; } = new();
}