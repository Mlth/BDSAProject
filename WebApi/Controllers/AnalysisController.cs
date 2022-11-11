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
        //var repositoryPath = "/Users/anton/Desktop/BDSAProject/";

        // Console.WriteLine(repositoryPathExample);


        var cloneOptions = new CloneOptions { BranchName = "master", Checkout = true };
        //var path = @"C:\Users\anton\Downloads\mypath";
        var path = Directory.GetCurrentDirectory();
        
        var newDir = path + "/" + repository_name;
        string existingRepo = path + "/" + repository_name;
        if(Directory.GetDirectories(path, repository_name).Length < 1){
            cloneRepository(repositoryPath, newDir);
        } else {
            pullRepository(existingRepo);
        }

        WebProgram webProgram = new WebProgram();
        var jsonString = webProgram.GetAnalysisJsonString(newDir, command);
        
        deleteDirectory(newDir);

        return jsonString;
    }

    public static void cloneRepository(string repositoryPath, string newDir){
        Repository.Clone(repositoryPath, newDir);
    }

    public static void pullRepository(string repositoryPath){
        var repository = new Repository(repositoryPath);
        var signature = new Signature("Lukas", "luel@itu.dk", DateTime.Now);
        Commands.Pull(repository, signature, null);
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