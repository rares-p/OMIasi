using Application.Contracts.Repositories;
using MediatR;

namespace Application.Features.Tests.Queries.GetAll;

public class GetAllTestsQueryHandler(ITestRepository testRepository) : IRequestHandler<GetAllTestsQuery, List<TestDto>>
{
    public async Task<List<TestDto>> Handle(GetAllTestsQuery request, CancellationToken cancellationToken)
    {
        var result = await testRepository.GetAllAsyncByProblemId(request.ProblemId);
        if (result.IsSuccess)
            return result.Value.Select(test => new TestDto { Id = test.Id, Score = test.Score }).ToList();
        return new();
    }
}