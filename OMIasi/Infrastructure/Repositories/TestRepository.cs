using Application.Contracts.Repositories;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TestRepository(OMIIasiDbContext context) : BaseRepository<Test>(context), ITestRepository
{
    private readonly OMIIasiDbContext _context = context;

    public async Task<Result<IReadOnlyList<Test>>> GetAllAsyncByProblemId(Guid problemId)
    {
        var result = await _context.Tests.Where(test => test.ProblemId == problemId).ToListAsync();
        return Result<IReadOnlyList<Test>>.Success(result);
    }
}