using Domain.Common;
using Domain.Entities;

namespace Application.Contracts.Repositories;

public interface IUserRepository : IAsyncRepository<User>
{
    Task<bool> ExistsAsync(string username);
    Task<Result<User>> FindByUserNameAsync(string username);
}