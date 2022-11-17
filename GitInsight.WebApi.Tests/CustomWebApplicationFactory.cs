namespace GitInsight.WebApi.Tests;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using GitInsight.WebApi.Controllers;

public class CustomWebApplicationFactory : WebApplicationFactory<WebApi.Program>, IAsyncLifetime
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            //Finding and removing database from service
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<RepositoryContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            services.AddDbContext<RepositoryContext>(options =>
            {
                options.UseSqlite(connection);
            });
            
            //Finding and removing IGitFetcher from service
            descriptor = services.SingleOrDefault(f => f.ServiceType == typeof(IGitFetcher));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddScoped<IGitFetcher, TestGitFetcher>();

        });

        builder.UseEnvironment("Development");
    }

    public async Task InitializeAsync(){
        using var scope = Services.CreateAsyncScope();
        using var context = scope.ServiceProvider.GetRequiredService<RepositoryContext>();
        if(context.Database.IsRelational()){
            await context.Database.EnsureCreatedAsync();
        }
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        using var scope = Services.CreateAsyncScope();
        using var context = scope.ServiceProvider.GetRequiredService<RepositoryContext>();

        await context.Database.EnsureDeletedAsync();
        //DeleteReadOnlyDirectory(@".\test-repo3\");
    }

    public void DeleteReadOnlyDirectory(string directoryPath)
    {
        foreach (var subdirectory in Directory.EnumerateDirectories(directoryPath)) 
        {
            DeleteReadOnlyDirectory(subdirectory);
        }
        foreach (var fileName in Directory.EnumerateFiles(directoryPath))
        {
            var fileInfo = new FileInfo(fileName);
            fileInfo.Attributes = FileAttributes.Normal;
            fileInfo.Delete();
        }
        Directory.Delete(directoryPath, true);
    }
}