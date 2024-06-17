using Application.Contracts;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProblemRepository(OMIIasiDbContext context) : IProblemRepository
{
    public async Task<Result<Problem>> FindByIdAsync(Guid id)
    {
        var problem = await context.Problems.FirstOrDefaultAsync(p => p.Id == id);
        if(problem == null)
            return Result<Problem>.Failure($"Problem with id {id} not found");

        return Result<Problem>.Success(problem);
    }

    public Task<Result<Problem>> FindByTitleAsync(string title)
    {
        throw new NotImplementedException();
    }
}