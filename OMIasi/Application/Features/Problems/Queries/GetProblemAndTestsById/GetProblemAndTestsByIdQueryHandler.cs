using Application.Contracts.Repositories;
using Application.Features.Tests;
using Domain.Common;
using MediatR;

namespace Application.Features.Problems.Queries.GetProblemAndTestsById;

public class GetProblemAndTestsByIdQueryHandler(IProblemRepository problemRepository, ITestRepository testRepository) : IRequestHandler<GetProblemAndTestsByIdQuery, Result<GetProblemAndTestsByIdQueryResponse>>
{
    public async Task<Result<GetProblemAndTestsByIdQueryResponse>> Handle(GetProblemAndTestsByIdQuery request, CancellationToken cancellationToken)
    {
        var problemResult = await problemRepository.FindByIdAsync(request.ProblemId);
        if (!problemResult.IsSuccess)
            return Result<GetProblemAndTestsByIdQueryResponse>.Failure(problemResult.Error);
        var testsResult = await testRepository.GetAllAsyncByProblemId(request.ProblemId);
        if (!testsResult.IsSuccess)
            return Result<GetProblemAndTestsByIdQueryResponse>.Failure(testsResult.Error);
        return Result<GetProblemAndTestsByIdQueryResponse>.Success(new()
        {
            Id = problemResult.Value.Id,
            Title = problemResult.Value.Title,
            Description = problemResult.Value.Description,
            NoTests = problemResult.Value.NoTests,
            Author = problemResult.Value.Author,
            TimeLimitInSeconds = problemResult.Value.TimeLimitInSeconds,
            TotalMemoryLimitInMb = problemResult.Value.TotalMemoryLimitInMb,
            StackMemoryLimitInMb = problemResult.Value.StackMemoryLimitInMb,
            Grade = problemResult.Value.Grade,
            InputFileName = problemResult.Value.InputFileName,
            OutputFileName = problemResult.Value.OutputFileName,
            Year = problemResult.Value.Year,
            Tests = testsResult.Value.Select(test => new TestDto()
            {
                Id = test.Id,
                Index = test.Index,
                Input = test.Input,
                Output = test.Output,
                Score = test.Score
            }).ToList()
        });
    }
}