using Application.Contracts.Repositories;
using MediatR;

namespace Application.Features.Tests.Queries.GetById;

public class GetTestByIdQueryHandler(ITestRepository testRepository, ITestContentRepository testContentRepository) : IRequestHandler<GetTestByIdQuery, GetTestByIdQueryResponse>
{
    public async Task<GetTestByIdQueryResponse> Handle(GetTestByIdQuery request, CancellationToken cancellationToken)
    {
        var testResult = await testRepository.FindByIdAsync(request.Id);
        if (!testResult.IsSuccess)
            return new GetTestByIdQueryResponse()
            {
                Success = false,
                Error = testResult.Error
            };

        var content = await testContentRepository.GetTest(testResult.Value.ProblemId, testResult.Value.Id);

        return new()
        {
            Success = true,
            Input = content.input,
            Output = content.output,
        };
    }
}