namespace GitInsight;
using GitInsight.Entities;
using LibGit2Sharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class Program
{
     public IConfiguration Configuration { get; }
    public Program(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    
    public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<RepositoryContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllersWithViews();
        }

    public static void Main(string[] args)
    {
        var repository = new Repository(args[0]);
        var command = Factory.getCommand(args[1]);
        command.execute(repository);
        var visualizer = command.getVisualizer();
        visualizer.visualize();
    }
}
