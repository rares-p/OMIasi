using Application.Models;
using Application.Responses;

namespace Application.Contracts;

public interface IEvaluationService
{
    Task<EvaluationResponse> Evaluate(Evaluation evaluation);
}