using Domain.Common;
using Domain.Entities;

namespace Application.Contracts.Repositories;

public interface ISubmissionRepository : IAsyncRepository<Submission>
{
    Task<Result<IReadOnlyCollection<Submission>>> GetSubmissionsByProblemIdAndUserId(Guid problemId, Guid userId);
}