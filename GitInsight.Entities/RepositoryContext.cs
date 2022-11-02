namespace GitInsight.Entities;
using Microsoft.EntityFrameworkCore;

public class RepositoryContext : DbContext
{
    public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
    {
    }

    public DbSet<RepositoryCommitData> CommitData { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RepositoryCommitData>().ToTable("ReposityCommitData");
    }
}