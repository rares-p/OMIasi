using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class OMIIasiDbContext(DbContextOptions<OMIIasiDbContext> options) : DbContext(options)
{
    public required DbSet<User> Users { get; init; }
    public required DbSet<Problem> Problems { get; init; }
    public required DbSet<Test> Tests { get; init; }
    public required DbSet<Submission> Submissions { get; init; }
    public required DbSet<SubmissionTestResult> SubmissionResults { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Submission>()
            .HasOne<Problem>()
            .WithMany()
            .HasForeignKey(s => s.ProblemId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Submission>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Submission>(entity =>
        {
            entity.OwnsMany(e => e.Scores, sa =>
            {
                sa.WithOwner().HasForeignKey("SubmissionId");
                sa.Property<Guid>("Id");
                sa.HasKey("Id");
                sa.Property(s => s.Message).HasColumnName("Message");
                sa.Property(s => s.Score).HasColumnName("Score");
            });
        });

        modelBuilder.HasDefaultSchema("omiiasi");
    }
}