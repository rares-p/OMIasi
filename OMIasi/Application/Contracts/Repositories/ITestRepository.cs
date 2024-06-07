using Domain.Common;
using Domain.Entities;

namespace Application.Contracts.Repositories;

public interface ITestRepository : IAsyncRepository<Test>
{
    Task<Result<IReadOnlyList<Test>>> GetAllAsyncByProblemId(Guid problemId);
}