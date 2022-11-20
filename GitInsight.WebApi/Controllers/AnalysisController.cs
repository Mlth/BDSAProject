using Microsoft.AspNetCore.Mvc;
using LibGit2Sharp;
using GitInsight;
using Octokit;
using GitInsight.Entities;

namespace GitInsight.WebApi.Controllers;

// Repository identifier example
// https://localhost:7024/analysis/Mlth/BDSAProject/author
// https://localhost:7024/analysis/Mlth/BDSAProject/frequency

[ApiController]
[Route("[controller]")]
public class AnalysisController : ControllerBase
{
    private readonly RepositoryContext context;
    private readonly IGitFetcher fetcher;

    string githubApiKey = Program.githubApiKey;

    public AnalysisController(RepositoryContext context, IGitFetcher fetcher){
        this.context = context;
        this.fetcher = fetcher;
    }

    [HttpGet("{github_user}/{repository_name}/{command}")]
    public async Task<string> Get(string github_user, string repository_name, string command)
    {
        var repositoryLink = "https://github.com/" + github_user + "/" + repository_name + ".git";
        var path = Directory.GetCurrentDirectory();

        var productInformation = new ProductHeaderValue("luel");
        var credentials = new Octokit.Credentials(githubApiKey);
        var client = new GitHubClient(productInformation) { Credentials = credentials };

        IReadOnlyList<Octokit.Repository> allForks = await client.Repository.Forks.GetAll(
            "Mlth", "BDSAProject");
        /*foreach (Octokit.Repository fork in allForks){
            Console.WriteLine(fork.CloneUrl);
        }*/
        
        var repositories = Directory.GetParent(Directory.GetCurrentDirectory()) + "/Repositories/";
        foreach(string s in Directory.GetDirectories(Directory.GetCurrentDirectory(), repository_name)){
            Console.WriteLine(s);
        }
        var repositoryLocation = repositories + repository_name;
        if(Directory.GetDirectories(repositories, repository_name).Length < 1){
            fetcher.cloneRepository(repositoryLink, repositoryLocation);
        } else {
            fetcher.pullRepository(repositoryLocation);
        }

        var repo = new LibGit2Sharp.Repository(repositoryLocation);
        var chosenCommand = Factory.getCommandAndIncjectDependencies(command, repo, context);
        chosenCommand.processRepo();
        var analysis = chosenCommand.getAnalysis();
        var jsonString = analysis.analyze();
        repo.Dispose();

        //deleteDirectory(repositoryLocation);
        
        return jsonString;

    }

    public static void deleteDirectory(string path)
    {
        foreach (var subdirectory in Directory.EnumerateDirectories(path))
        {
            deleteDirectory(subdirectory);
        }
        foreach (var fileName in Directory.EnumerateFiles(path))
        {
            var fileInfo = new FileInfo(fileName);
            fileInfo.Attributes = FileAttributes.Normal;
            fileInfo.Delete();
        }
        var dir = new DirectoryInfo(path);
        dir.Delete(true);
    }
}