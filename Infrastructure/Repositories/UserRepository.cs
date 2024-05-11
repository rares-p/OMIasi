using Application.Contracts.Repositories;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class UserRepository(OMIIasiDbContext context) : BaseRepository<User>(context), IUserRepository;