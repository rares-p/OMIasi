using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class OMIIasiDbContext(DbContextOptions<OMIIasiDbContext> options) : DbContext(options)
{
    public required DbSet<User> Users { get; init; }
    public required DbSet<Problem> Problems { get; init; }
    public required DbSet<Test> Tests { get; init; }
    public required DbSet<Submission> Submissions { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("omiiasi");
    }
}