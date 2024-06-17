using Application.Contracts.Repositories;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class SubmissionRepository(OMIIasiDbContext context) : BaseRepository<Submission>(context), ISubmissionRepository
{
    public async Task<Result<IReadOnlyCollection<Submission>>> GetSubmissionsByProblemIdAndUserId(Guid problemId, Guid userId)
    {
        var result = await context.Submissions.Where(s => s.ProblemId == problemId && s.UserId == userId).ToListAsync();
        if(result == null)
            return Result<IReadOnlyCollection<Submission>>.Failure("Error");
        return Result<IReadOnlyCollection<Submission>>.Success(result);
    }
}