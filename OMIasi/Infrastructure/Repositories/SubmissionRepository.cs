using Application.Contracts.Repositories;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class SubmissionRepository(OMIIasiDbContext context) : BaseRepository<Submission>(context), ISubmissionRepository;