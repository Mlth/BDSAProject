namespace GitInsight.WebApi.Tests;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;

public class CustomWebApplicationFactory : WebApplicationFactory<WebApi.Program>, IAsyncLifetime
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<RepositoryContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

             var connectionString = $"Server=localhost,1433;Database=GitInsightDB;User Id=SA;Password=<YourStrong@Passw0rd>;Trusted_Connection=False;Encrypt=False";

            services.AddDbContext<RepositoryContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
        });

        builder.UseEnvironment("Development");
    }

    public async Task InitializeAsync(){
        using var scope = Services.CreateAsyncScope();
        using var context = scope.ServiceProvider.GetRequiredService<RepositoryContext>();
        await context.Database.MigrateAsync();

        /*
        var path = @".\test-repo3\";
        Repository.Init(path);
        var repo = new Repository(path);

        var commitDateTime1 = DateTimeOffset.Now;
        var author1 = new Signature("mlth", "mlth@itu.dk", commitDateTime1);
        var commitDateTime2 = DateTimeOffset.Now;
        var author2 = new Signature("aarv", "aarv@itu.dk", commitDateTime2);
        var command = new FrequencyCommand();

        repo.Commit("First commit", author1, author1, new CommitOptions(){ AllowEmptyCommit = true });

        command.template(repo, context);

        await context.SaveChangesAsync();
        */
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        using var scope = Services.CreateAsyncScope();
        using var context = scope.ServiceProvider.GetRequiredService<RepositoryContext>();

        await context.Database.EnsureDeletedAsync();
    }
}