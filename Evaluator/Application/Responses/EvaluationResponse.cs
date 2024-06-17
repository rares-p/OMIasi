using Application.Models;

namespace Application.Responses;

public class EvaluationResponse : BaseResponse
{
    public required List<TestResultModel> Results { get; set; }
}