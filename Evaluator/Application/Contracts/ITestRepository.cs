using Domain.Common;
using Domain.Entities;

namespace Application.Contracts;

public interface ITestRepository
{
    Task<Result<IReadOnlyList<Test>>> GetTestsByProblemIdAsync(Guid id);
}