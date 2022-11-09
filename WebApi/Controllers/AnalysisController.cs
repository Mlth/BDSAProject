using Microsoft.AspNetCore.Mvc;
using LibGit2Sharp;
using GitInsight;
using System;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AnalysisController : ControllerBase
{
    [HttpGet("{github_user}/{repository_name}/{command}")]
    public List<string> Get(string github_user, string repository_name, string command)
    {
        var repositoryPath = "https://github.com/" + github_user + "/" + repository_name;
        // var repositoryPath = "/Users/anton/Desktop/BDSAProject/";

        Console.WriteLine(repositoryPath);

        WebProgram webProgram = new WebProgram();
        var list = webProgram.GetAnalysisList(repositoryPath, command); 

        return list;
    }
}
