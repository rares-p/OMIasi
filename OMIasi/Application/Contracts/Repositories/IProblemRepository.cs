using Domain.Common;
using Domain.Entities;

namespace Application.Contracts.Repositories;

public interface IProblemRepository : IAsyncRepository<Problem>
{
    Task<bool> ExistsAsync(string name);
    Task<Result<Problem>> FindByTitleAsync(string title);
}