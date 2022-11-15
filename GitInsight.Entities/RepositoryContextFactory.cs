namespace GitInsight.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
{
    public RepositoryContext CreateDbContext(string[] args)
    {
        string _conStr = @"
            Server=localhost,1433;
            Database=GitInsightDB2;
            User Id=SA;
            Password=<YourStrong@Passw0rd>;";

        var optionsBuilder = new DbContextOptionsBuilder<RepositoryContext>();
        optionsBuilder.UseSqlServer(_conStr);

        return new RepositoryContext(optionsBuilder.Options);
    }
}