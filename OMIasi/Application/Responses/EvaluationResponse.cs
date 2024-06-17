using Application.Models;

namespace Application.Responses;

public class EvaluationResponse : BaseResponse
{
    public List<TestResultModel> Results { get; set; }
}