namespace GitInsight;
using Octokit;
using GitInsight.Entities.DTOS;
using System.Text.Json;

public class ForkCommand {


    public async Task<IEnumerable<ForkDTO>> analyzeForks(string githubApiKey, string username, string repoName){
        var productInformation = new ProductHeaderValue("luel");
        var credentials = new Octokit.Credentials(githubApiKey);
        var client = new GitHubClient(productInformation) { Credentials = credentials };

        IReadOnlyList<Octokit.Repository> allForks = await client.Repository.Forks.GetAll("Mlth", "BDSAProject");
        return from f in allForks select new ForkDTO{url = f.CloneUrl};
    }

    public async Task<string> getJsonString(string githubApiKey, string username, string repoName)
    {
        var forks = await analyzeForks(githubApiKey, username, repoName);
        var options = new JsonSerializerOptions { WriteIndented = true };
        var jsonString = JsonSerializer.Serialize(forks, options);
        return jsonString;
    }
}