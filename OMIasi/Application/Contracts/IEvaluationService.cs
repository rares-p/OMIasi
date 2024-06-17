using Application.Models;
using Domain.Common;

namespace Application.Contracts;

public interface IEvaluationService
{
    Task<Result<List<TestResultModel>>> Evaluate(Guid problemId, string solution);
}