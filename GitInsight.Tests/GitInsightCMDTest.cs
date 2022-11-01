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
    public void Factory_returns_correct_command(){
        //Arrange
        string frequencyString = "frequency";
        string authorString = "author";
        string exceptionString = "test";

        //Act
        var frequencyCommand = Factory.getCommand(frequencyString);
        var authorCommand = Factory.getCommand(authorString);

        //Assert
        Assert.IsType<FrequencyCommand>(frequencyCommand);
        Assert.IsType<AuthorCommand>(authorCommand);
        try{
            Factory.getCommand(exceptionString);
            Assert.Fail("Exception was not thrown when using invalid string for Factory.GetCommand");
        } catch(NotImplementedException e){

        }
    }

    [Fact]
    public void Repo_has_no_commits_so_frequency_command_contains_no_frequencyDTO(){
        //Arrange
        var command = (FrequencyCommand)Factory.getCommand("frequency");

        //Act
        command.execute(repo);

        //Assert
        Assert.Equal(0, command.frequencies.Count);
    }

    [Fact]
    public void Repo_has_one_commit_by_one_author_so_frequency_command_contains_one_frequencyDTO(){
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
        Assert.Equal(1, command.frequencies.Count);
    }

    [Fact]
    public void Repo_has_one_commit_by_two_authors_so_frequency_command_contains_one_frequencyDTO_with_frequency_of_two(){
        //Arrange
        var commitDateTime1 = DateTimeOffset.Now;
        var author1 = new Signature("mlth", "mlth@itu.dk", commitDateTime1);
        var commitDateTime2 = DateTimeOffset.Now;
        var author2 = new Signature("aarv", "aarv@itu.dk", commitDateTime2);
        var command = (FrequencyCommand)Factory.getCommand("frequency");

        //Act
        repo.Commit("First commit", author1, author1, new CommitOptions(){ AllowEmptyCommit = true });
        repo.Commit("Second commit", author2, author2, new CommitOptions(){ AllowEmptyCommit = true });
        command.execute(repo);

        //Assert
        Assert.Equal(2, command.frequencies[0].frequency);
        Assert.Equal(commitDateTime1.Date.ToShortDateString(), command.frequencies[0].dateTime);
        Assert.Equal(1, command.frequencies.Count);
    }

    //Test FrequencyDTO with multiple commits by one author

    //Test FrequencyDTO with multiple commits by multiple authors

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