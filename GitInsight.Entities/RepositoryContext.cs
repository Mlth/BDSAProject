namespace GitInsight.Entities;
using Microsoft.EntityFrameworkCore;

public class RepositoryContext : DbContext
{
    public DbSet<RepositoryCommitData> CommitData { get; set; }

    public string DbPath { get; }

    public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options)
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "repodata.db");
    }


    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RepositoryCommitData>().ToTable("ReposityCommitData");
    }
}