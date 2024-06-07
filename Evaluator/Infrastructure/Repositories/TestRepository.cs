using Application.Contracts;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TestRepository(OMIIasiDbContext context) : ITestRepository
{
    public async Task<Result<IReadOnlyList<Test>>> GetTestsByProblemIdAsync(Guid id)
    {
        var result = await context.Tests.Where(test => test.ProblemId == id).ToListAsync();
        return result == null ? Result<IReadOnlyList<Test>>.Failure("No tests found!") : Result<IReadOnlyList<Test>>.Success(result);
    }
}