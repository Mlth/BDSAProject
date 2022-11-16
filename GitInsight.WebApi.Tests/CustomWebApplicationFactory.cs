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

            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open();

            services.AddDbContext<RepositoryContext>(options =>
            {
                options.UseSqlite(connection);
            });
        });

        builder.UseEnvironment("Development");
    }

    public async Task InitializeAsync(){
        using var scope = Services.CreateAsyncScope();
        using var context = scope.ServiceProvider.GetRequiredService<RepositoryContext>();
        if(context.Database.IsRelational()){
            await context.Database.EnsureCreatedAsync();
        }

        /*var testRepoToBeCloned = "https://github.com/Mlth/test-repo3";
        var pathToPutClonedRepo = @".\test-repo3\";
        var pathToClonedRepo = Repository.Clone(testRepoToBeCloned, pathToPutClonedRepo);
        var repo = new Repository(pathToClonedRepo);

        var commitDateTime1 = DateTimeOffset.Now;
        var author1 = new Signature("mlth", "mlth@itu.dk", commitDateTime1);
        var command = new FrequencyCommand();

        repo.Commit("First commit", author1, author1, new CommitOptions(){ AllowEmptyCommit = true });

        Remote remote = repo.Network.Remotes["origin"];
        var pushRefSpec = @"refs/heads/master";              
        repo.Network.Push(remote, pushRefSpec);

        await context.SaveChangesAsync();*/
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