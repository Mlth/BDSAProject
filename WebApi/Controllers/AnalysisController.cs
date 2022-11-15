using Microsoft.AspNetCore.Mvc;
using LibGit2Sharp;
using GitInsight;

namespace WebApi.Controllers;

// Repository identifier example
// https://localhost:7024/analysis/Mlth/BDSAProject/author
// https://localhost:7024/analysis/Mlth/BDSAProject/frequency

[ApiController]
[Route("[controller]")]
public class AnalysisController : ControllerBase
{
    [HttpGet("{github_user}/{repository_name}/{command}")]
    public string Get(string github_user, string repository_name, string command)
    {
        var repositoryPath = "https://github.com/" + github_user + "/" + repository_name + ".git";
        var path = Directory.GetCurrentDirectory();

        WebProgram webProgram = new WebProgram();
        var jsonString = webProgram.GetAnalysisJsonString(repositoryPath, repository_name, command);

        return jsonString;
    }
}