namespace GitInsight.WebApi;
using GitInsight.Entities;
using Microsoft.EntityFrameworkCore;

public class Program{
    public static void Main(String[] args){
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddDbContext<RepositoryContext>(opt => opt.UseSqlServer(@"
            Server=localhost,1433;
            Database=GitInsightDB;
            User Id=SA;
            Password=<YourStrong@Passw0rd>;
            Trusted_Connection=False;
            Encrypt=False"));
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddScoped<IGitFetcher, GitFetcher>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
