using Application.Contracts.Repositories;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository(OMIIasiDbContext context) : BaseRepository<User>(context), IUserRepository
{
    private readonly OMIIasiDbContext _context = context;

    public async Task<bool> ExistsAsync(string username)
    {
        return await _context.Users.AnyAsync(user => user.Username == username);
    }

    public async Task<Result<User>> FindByUserNameAsync(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Username == username);
        return user == null
            ? Result<User>.Failure($"User with username {username} not found!")
            : Result<User>.Success(user);
    }
}