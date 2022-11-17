namespace WebApi;

public partial class Program
{
    public static string githubApiKey;

    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        //dotnet user-secrets init
        //dotnet user-secrets set "Tokens:gitinsight" "YOUR_TOKEN"
        

        githubApiKey = builder.Configuration["Tokens:gitinsight"];

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.MapGet("/", () => githubApiKey);

        app.Run();

    }

}