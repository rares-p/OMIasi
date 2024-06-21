using Application.Contracts.Repositories;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProblemRepository(OMIIasiDbContext context) : BaseRepository<Problem>(context), IProblemRepository
{
    private readonly OMIIasiDbContext _context = context;

    public async Task<bool> ExistsAsync(string title) => await _context.Problems.AnyAsync(problem => problem.Title == title);

    public async Task<Result<Problem>> FindByTitleAsync(string title)
    {
        var problem = await _context.Problems.FirstOrDefaultAsync(problem => problem.Title == title);
        return problem != null
            ? Result<Problem>.Success(problem)
            : Result<Problem>.Failure($"Problem with title {title} not found!");
    }

    public async Task<Result<Problem>> GetProblemWithTestsById(Guid id)
    {
        var problem = await _context.Problems.Include(problem => problem.Tests).FirstOrDefaultAsync(problem => problem.Id == id);
        if(problem == null)
            return Result<Problem>.Failure("Problem not found");
        return Result<Problem>.Success(problem);
    }
}