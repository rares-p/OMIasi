using Application.Contracts.Repositories;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class ProblemRepository(OMIIasiDbContext context) : BaseRepository<Problem>(context), IProblemRepository;