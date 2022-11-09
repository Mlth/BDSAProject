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
        // https://localhost:7024/analysis/Mlth/BDSAProject/author

        var repositoryPathExample = "https://github.com/" + github_user + "/" + repository_name + ".git";
        var repositoryPath = "/Users/anton/Desktop/BDSAProject/";

        Console.WriteLine(repositoryPathExample);

        var cloneOptions = new CloneOptions { BranchName = "master", Checkout = true };
        var cloneResult = Repository.Clone( repositoryPathExample, @"C:\Users\anton\Downloads\mypath" );

        WebProgram webProgram = new WebProgram();
        var list = webProgram.GetAnalysisList(repositoryPath, command); 

        return list;
    }
}