using Domain.Common;
using Domain.Entities;

namespace Application.Contracts;

public interface IProblemRepository
{
    Task<Result<Problem>> FindByIdAsync(Guid id);
    Task<Result<Problem>> FindByTitleAsync(string title);
}