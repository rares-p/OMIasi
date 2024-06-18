using Application.Contracts.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.Problems.Commands.CreateProblem;

public class CreateProblemCommandHandler(IProblemRepository problemRepository, ITestRepository testRepository) : IRequestHandler<CreateProblemCommand, CreateProblemCommandResponse>
{
    public async Task<CreateProblemCommandResponse> Handle(CreateProblemCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateProblemCommandValidator();
        var validatorResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validatorResult.IsValid)
            return new CreateProblemCommandResponse()
            {
                Success = false,
                Error = string.Join('\n', validatorResult.Errors.Select(x => x.ErrorMessage).ToList())
            };

        if (await problemRepository.ExistsAsync(request.Title))
            return new CreateProblemCommandResponse()
            {
                Success = false,
                Error = $"Problem with Title {request.Title} already exists!"
            };

        var problem = Problem.Create(request.Title, request.Description, request.NoTests, request.Author,
            request.TimeLimitInSeconds, request.TotalMemoryLimitInMb, request.StackMemoryLimitInMb, request.Grade,
            request.InputFileName, request.OutputFileName, request.Year);

        if (!problem.IsSuccess)
            return new CreateProblemCommandResponse()
            {
                Success = false,
                Error = problem.Error
            };

        var tests = request.Tests.Select(test => Test.Create(problem.Value.Id, test.Index, test.Input, test.Output, test.Score)).ToList();

        if (tests.Any(test => !test.IsSuccess))
            return new CreateProblemCommandResponse()
            {
                Success = false,
                Error = string.Join('\n', tests.Select(test => test.Error).ToList())
            };

        try
        {
            await problemRepository.AddAsync(problem.Value);
            foreach (var test in tests) await testRepository.AddAsync(test.Value);
        }
        catch (Exception)
        {
            return new CreateProblemCommandResponse()
            {
                Success = false,
                Error = "Server Error"
            };
        }

        return new CreateProblemCommandResponse()
        {
            Success = true,
            Problem = new ProblemDto()
            {
                Id = problem.Value.Id,
                Title = problem.Value.Title,
                Author = problem.Value.Author,
                Year = problem.Value.Year,
                Description = problem.Value.Description,
                Grade = problem.Value.Grade,
                InputFileName = problem.Value.InputFileName,
                OutputFileName = problem.Value.OutputFileName,
                NoTests = problem.Value.NoTests,
                StackMemoryLimitInMb = problem.Value.StackMemoryLimitInMb,
                TotalMemoryLimitInMb = problem.Value.TotalMemoryLimitInMb,
                TimeLimitInSeconds = problem.Value.TimeLimitInSeconds
            }
        };
    }
}