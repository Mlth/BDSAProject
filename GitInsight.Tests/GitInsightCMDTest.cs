namespace GitInsight.Tests;
using LibGit2Sharp;
using System.IO;
using GitInsight;

public class GitInsightCMDTest : IDisposable
{
    Repository repo;
    string path = @".\test-repo\";

    public GitInsightCMDTest(){
        Repository.Init(path);
        repo = new Repository(path);
    }

    [Fact]
    public void Repo_has_one_commit_so_frequency_command_contains_one_frequencyDTO()
    {
        //Arrange
        var commitDateTime1 = DateTimeOffset.Now;
        var author1 = new Signature("mlth", "mlth@itu.dk", commitDateTime1);
        var command = (FrequencyCommand)Factory.getCommand("frequency");

        //Act
        repo.Commit("First commit", author1, author1, new CommitOptions(){ AllowEmptyCommit = true });
        command.execute(repo);

        //Assert
        Assert.Equal(1, command.frequencies[0].frequency);
        Assert.Equal(commitDateTime1.Date.ToShortDateString(), command.frequencies[0].dateTime);
    }

    //Test that factory returns correct comand

    //Test FrequencyDTO with multiple commits

    //Test FrequencyDTO with no commits

    //Test AuthorDTO with one commit and one author

    //AnDerS
    //Test AuthorDTO with one commit by multiple authors

    //Test AuthorDTO with multiple commits by one author

    //Test AuthorDTO with multiple commits by multiple authors

    //Test AuthorDTO with no commits

    public void Dispose(){
        repo.Dispose();
        DeleteReadOnlyDirectory(path);
    }

    public void DeleteReadOnlyDirectory(string directoryPath)
    {
        foreach (var subdirectory in Directory.EnumerateDirectories(directoryPath)) 
        {
            DeleteReadOnlyDirectory(subdirectory);
        }
        foreach (var fileName in Directory.EnumerateFiles(directoryPath))
        {
            var fileInfo = new FileInfo(fileName);
            fileInfo.Attributes = FileAttributes.Normal;
            fileInfo.Delete();
        }
        Directory.Delete(directoryPath, true);
    }
}