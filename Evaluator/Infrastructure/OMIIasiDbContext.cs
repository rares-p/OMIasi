using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class OMIIasiDbContext(DbContextOptions<OMIIasiDbContext> options) : DbContext(options)
{
    public required DbSet<Test> Tests { get; init; }
    public required DbSet<Problem> Problems { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("omiiasi");
        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        throw new InvalidOperationException("Read-only context does not support changes.");
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new InvalidOperationException("Read-only context does not support changes.");
    }
}