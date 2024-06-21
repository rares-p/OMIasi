using Application.Contracts.Repositories;
using Domain.Common;
using Domain.Entities;
using MediatR;

namespace Application.Features.Problems.Commands.Update;

public class UpdateProblemCommandHandler(IProblemRepository problemRepository, ITestRepository testRepository, ITestContentRepository testContentRepository) : IRequestHandler<UpdateProblemCommand, UpdateProblemCommandResponse>
{
    public async Task<UpdateProblemCommandResponse> Handle(UpdateProblemCommand request, CancellationToken cancellationToken)
    {
        var problemResult = await problemRepository.FindByIdAsync(request.Id);
        if (!problemResult.IsSuccess)
            return new()
            {
                Success = false,
                Error = problemResult.Error
            };

        var testsResult = await testRepository.GetAllAsyncByProblemId(request.Id);
        if(!testsResult.IsSuccess)
            return new()
            {
                Success = false,
                Error = testsResult.Error
            };

        if (request.Tests.Sum(test => test.Score) != 100)
            return new UpdateProblemCommandResponse()
            {
                Success = false,
                Error = "Scores must add up to 100"
            };

        if (request.Tests.Where(test => test.Id is null).Any(test => test.Input is null || test.Output is null))
            return new UpdateProblemCommandResponse()
            {
                Success = false,
                Error = "Content for created tests cannot be empty"
            };

        foreach (var test in request.Tests.Where(test => test.Id != null))
        {
            var testResult = await testRepository.FindByIdAsync((Guid)test.Id!);
            if (!testResult.IsSuccess)
                return new()
                {
                    Success = false,
                    Error = "Test not found!"
                };
            if (testResult.Value.ProblemId != request.Id)
                return new()
                {
                    Success = false,
                    Error = "Cannot modify test that belongs to another problem!"
                };
        }
        foreach (var test in testsResult.Value)
            if (request.Tests.All(reqTest => reqTest.Id != test.Id))
            {
                await testRepository.DeleteAsync(test.Id);
                await testContentRepository.DeleteTest(test.ProblemId, test.Id);
            }

        var updateProblemResult = UpdateProblem(problemResult, request);
        if (!updateProblemResult.IsSuccess)
            return new()
            {
                Success = false,
                Error = updateProblemResult.Error
            };

        var updateTestResults = new List<Result<Test>>();
        var createTestResults = new List<Result<Test>>();

        foreach (var test in request.Tests)
        {
            if (test.Id is not null)
            {
                var testResult = await testRepository.FindByIdAsync((Guid)test.Id);
                var updateTestResult = UpdateTest(testResult, request.Tests.First(t => t.Id == testResult.Value.Id));
                if (!updateTestResult.IsSuccess)
                    return new UpdateProblemCommandResponse()
                    {
                        Success = false,
                        Error = updateTestResult.Error
                    };
                if(test.Input is not null)
                 await testContentRepository.UpdateInput(problemResult.Value.Id, (Guid)test.Id, test.Input);
                if (test.Output is not null)
                    await testContentRepository.UpdateOutput(problemResult.Value.Id, (Guid)test.Id, test.Output);
                updateTestResults.Add(updateTestResult);
            }
            else
            {
                var testResult = Test.Create(request.Id, test.Index, test.Score);
                if (!testResult.IsSuccess)
                    return new UpdateProblemCommandResponse()
                    {
                        Success = false,
                        Error = testResult.Error
                    };
                createTestResults.Add(testResult);
                if (test.Input is not null && test.Output is not null)
                    await testContentRepository.CreateTest(problemResult.Value.Id, testResult.Value.Id, test.Input,
                        test.Output);
            }
        }

        if (updateTestResults.Any(result => !result.IsSuccess))
            return new()
            {
                Success = false,
                ValidationsErrors = updateTestResults.Select(result => result.Error).ToList()
            };


        try
        {
            await problemRepository.UpdateAsync(problemResult.Value);
            foreach (var test in updateTestResults) await testRepository.UpdateAsync(test.Value);
            foreach (var test in createTestResults) await testRepository.AddAsync(test.Value);
        }
        catch (Exception e)
        {
            return new()
            {
                Success = false,
                Error = e.Message
            };
        }

        return new()
        {
            Success = true
        };
    }

    private Result<Problem> UpdateProblem(Result<Problem> problemResult, UpdateProblemCommand request)
    {
        var updateTitleResult = problemResult.Value.UpdateTitle(request.Title);
        if (!updateTitleResult.IsSuccess)
            return Result<Problem>.Failure(updateTitleResult.Error);

        var updateDescriptionResult = problemResult.Value.UpdateDescription(request.Description);
        if (!updateDescriptionResult.IsSuccess)
            return Result<Problem>.Failure(updateDescriptionResult.Error);

        var updateNoTestsResult = problemResult.Value.UpdateNoTests(request.NoTests);
        if (!updateNoTestsResult.IsSuccess)
            return Result<Problem>.Failure(updateNoTestsResult.Error);

        var updateAuthorResult = problemResult.Value.UpdateAuthor(request.Author);
        if (!updateAuthorResult.IsSuccess)
            return Result<Problem>.Failure(updateAuthorResult.Error);

        var updateTimeLimitInSecondsResult = problemResult.Value.UpdateTimeLimitInSeconds(request.TimeLimitInSeconds);
        if (!updateTimeLimitInSecondsResult.IsSuccess)
            return Result<Problem>.Failure(updateTimeLimitInSecondsResult.Error);

        var updateTotalMemoryLimitInMbResult = problemResult.Value.UpdateTotalMemoryLimitInMb(request.TotalMemoryLimitInMb);
        if (!updateTotalMemoryLimitInMbResult.IsSuccess)
            return Result<Problem>.Failure(updateTotalMemoryLimitInMbResult.Error);

        var updateStackMemoryLimitInMbResult = problemResult.Value.UpdateStackMemoryLimitInMb(request.StackMemoryLimitInMb);
        if (!updateStackMemoryLimitInMbResult.IsSuccess)
            return Result<Problem>.Failure(updateStackMemoryLimitInMbResult.Error);

        var updateGradeResult = problemResult.Value.UpdateGrade(request.Grade);
        if (!updateGradeResult.IsSuccess)
            return Result<Problem>.Failure(updateGradeResult.Error);

        var updateInputFileNameResult = problemResult.Value.UpdateInputFileName(request.InputFileName);
        if (!updateInputFileNameResult.IsSuccess)
            return Result<Problem>.Failure(updateInputFileNameResult.Error);

        var updateOutputFileNameResult = problemResult.Value.UpdateOutputFileName(request.OutputFileName);
        if (!updateOutputFileNameResult.IsSuccess)
            return Result<Problem>.Failure(updateOutputFileNameResult.Error);

        var updateYearResult = problemResult.Value.UpdateYear(request.Year);
        if (!updateYearResult.IsSuccess)
            return Result<Problem>.Failure(updateYearResult.Error);

        return Result<Problem>.Success(problemResult.Value);
    }

    private Result<Test> UpdateTest(Result<Test> testResult, UpdateProblemTestModel request)
    {
        var updateIndexResult = testResult.Value.UpdateIndex(request.Index);
        if(!updateIndexResult.IsSuccess)
            return Result<Test>.Failure(updateIndexResult.Error);

        //var updateInputResult = testResult.Value.UpdateInput(request.Input);
        //if(!updateInputResult.IsSuccess)
        //    return Result<Test>.Failure(updateInputResult.Error);

        //var updateOutputResult = testResult.Value.UpdateOutput(request.Output);
        //if (!updateOutputResult.IsSuccess)
        //    return Result<Test>.Failure(updateOutputResult.Error);

        var updateScoreResult = testResult.Value.UpdateScore(request.Score);
        if(!updateScoreResult.IsSuccess)
            return Result<Test>.Failure(updateScoreResult.Error);

        return Result<Test>.Success(testResult.Value);
    }
}