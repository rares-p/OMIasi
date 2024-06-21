using Application.Contracts.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.Tests.Commands.Create;

public class CreateTestCommandHandler(IProblemRepository problemRepository, ITestRepository testRepository, ITestContentRepository testContentRepository) : IRequestHandler<CreateTestCommand, CreateTestCommandResponse>
{
    public async Task<CreateTestCommandResponse> Handle(CreateTestCommand request, CancellationToken cancellationToken)
    {
        var problemResult = await problemRepository.FindByIdAsync(request.problemId);
        if (!problemResult.IsSuccess)
            return new CreateTestCommandResponse()
            {
                Success = false,
                Error = problemResult.Error
            };

        var testResult = Test.Create(request.problemId, problemResult.Value.NoTests, 0);
        if (!testResult.IsSuccess)
            return new CreateTestCommandResponse()
            { Success = false, Error = testResult.Error };

        var problemUpdateNoTestsResult = problemResult.Value.UpdateNoTests(problemResult.Value.NoTests + 1);

        if (!problemUpdateNoTestsResult.IsSuccess)
            return new CreateTestCommandResponse()
            { Success = false, Error = problemUpdateNoTestsResult.Error };

        var testContentResult =
            await testContentRepository.CreateTest(request.problemId, testResult.Value.Id, request.input, request.output);

        if (!testContentResult)
            return new() { Success = false, Error = "Invalid test content" };

        try
        {
            await problemRepository.UpdateAsync(problemUpdateNoTestsResult.Value);
            await testRepository.AddAsync(testResult.Value);
        }
        catch (Exception e)
        {
            return new() { Success = false, Error = "Server error" };
        }

        return new CreateTestCommandResponse()
        {
            Success = true
        };
    }
}