using Application.Contracts.Repositories;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class TestRepository(OMIIasiDbContext context) : BaseRepository<Test>(context), ITestRepository;