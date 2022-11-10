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
        var repositoryPathExample = "https://github.com/" + github_user + "/" + repository_name + ".git";
        var repositoryPath = "/Users/anton/Desktop/BDSAProject/";

        // Console.WriteLine(repositoryPathExample);

        var cloneOptions = new CloneOptions { BranchName = "master", Checkout = true };
        //var path = @"C:\Users\anton\Downloads\mypath";
        var path = Directory.GetCurrentDirectory();
        var newDir = path + "/mypath";
        var cloneResult = Repository.Clone(repositoryPathExample, newDir);

        WebProgram webProgram = new WebProgram();
        var jsonString = webProgram.GetAnalysisJsonString(repositoryPath, command);

        deleteDirectory(newDir);

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