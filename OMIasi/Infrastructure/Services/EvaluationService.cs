using Application.Contracts;
using Microsoft.Extensions.Options;
using Infrastructure.Settings;
using System.Text;
using Application.Models;
using Application.Responses;
using Domain.Common;
using System.Text.Json;

namespace Infrastructure.Services;

public class EvaluationService(HttpClient httpClient, IOptions<EvaluationServiceSettings> settings)
    : IEvaluationService
{
    private readonly string _baseUrl = settings.Value.BaseUrl;

    public async Task<Result<List<TestResultModel>>> Evaluate(Guid problemId, string solution)
    {
        var data = new
        {
            ProblemId = problemId,
            Solution = solution
        };
        var requestResult = await httpClient.PostAsync($"{_baseUrl}/evaluate",
            new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json"));
        var test = await requestResult.Content.ReadAsStringAsync();
        var content = JsonSerializer.Deserialize<EvaluationResponse>(await requestResult.Content.ReadAsStringAsync(),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (content == null)
            return Result<List<TestResultModel>>.Failure("No content");

        if (!requestResult.IsSuccessStatusCode)
            return Result<List<TestResultModel>>.Failure(content.Error);

        return Result<List<TestResultModel>>.Success(content.Results);
    }
}