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
        } catch(NotImplementedException e){ }
    }

    [Fact]
    public void no_commits_returns_frequency_command_containing_no_frequencyDTO(){
        //Arrange
        var command = (FrequencyCommand)Factory.getCommand("frequency");

        //Act
        command.execute(repo);

        //Assert
        Assert.Equal(0, command.frequencies.Count);
    }

    [Fact]
    public void One_commit_by_one_author_returns_frequency_command_containing_one_frequencyDTO(){
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
    public void One_commit_by_two_authors_returns_frequency_command_containing_one_frequencyDTO_with_frequency_of_two(){
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

    [Fact]
    public void Two_commits_by_one_author_returns_frequency_command_containing_one_frequencyDTO_with_frequency_of_two(){
        //Arrange
        var commitDateTime1 = DateTimeOffset.Now;
        var author1 = new Signature("mlth", "mlth@itu.dk", commitDateTime1);
        var command = (FrequencyCommand)Factory.getCommand("frequency");

        //Act
        repo.Commit("First commit", author1, author1, new CommitOptions(){ AllowEmptyCommit = true });
        repo.Commit("Second commit", author1, author1, new CommitOptions(){ AllowEmptyCommit = true });
        command.execute(repo);

        //Assert
        Assert.Equal(2, command.frequencies[0].frequency);
        Assert.Equal(commitDateTime1.Date.ToShortDateString(), command.frequencies[0].dateTime);
        Assert.Equal(1, command.frequencies.Count);
    }

    [Fact]
    public void Two_commits_by_two_authors_returns_frequency_command_containing_one_frequencyDTO_with_frequency_of_four(){
        //Arrange
        var commitDateTime1 = DateTimeOffset.Now;
        var author1 = new Signature("mlth", "mlth@itu.dk", commitDateTime1);
        var commitDateTime2 = DateTimeOffset.Now;
        var author2 = new Signature("aarv", "aarv@itu.dk", commitDateTime2);
        var commitDateTime3 = DateTimeOffset.Now;
        var author3 = new Signature("mlth", "mlth@itu.dk", commitDateTime3);
        var commitDateTime4 = DateTimeOffset.Now;
        var author4 = new Signature("aarv", "aarv@itu.dk", commitDateTime4);
        var command = (FrequencyCommand)Factory.getCommand("frequency");

        //Act
        repo.Commit("First commit", author1, author1, new CommitOptions(){ AllowEmptyCommit = true });
        repo.Commit("Second commit", author2, author2, new CommitOptions(){ AllowEmptyCommit = true });
        repo.Commit("Third commit", author3, author3, new CommitOptions(){ AllowEmptyCommit = true });
        repo.Commit("Fourth commit", author4, author4, new CommitOptions(){ AllowEmptyCommit = true });
        command.execute(repo);

        //Assert
        Assert.Equal(4, command.frequencies[0].frequency);
        Assert.Equal(commitDateTime1.Date.ToShortDateString(), command.frequencies[0].dateTime);
        Assert.Equal(1, command.frequencies.Count);
    }

    [Fact]
    public void One_commit_by_one_author_returns_author_command_containing_one_author_with_one_frequencyDTO_with_frequency_of_one(){
        //Arrange
        var commitDateTime1 = DateTimeOffset.Now;
        var author1 = new Signature("mlth", "mlth@itu.dk", commitDateTime1);
        var command = (AuthorCommand)Factory.getCommand("author");

        //Act
        repo.Commit("First commit", author1, author1, new CommitOptions(){ AllowEmptyCommit = true });
        command.execute(repo);

        //Assert
        Assert.Equal("mlth", command.authors[0].Author);
        Assert.Equal(1, command.authors[0].frequencies[0].frequency);
        Assert.Equal(commitDateTime1.Date.ToShortDateString(), command.authors[0].frequencies[0].dateTime);
        Assert.Equal(1, command.authors.Count);
    }
    //Test AuthorDTO with one commit by multiple authors
    [Fact]
    public void One_commit_by_multiple_authors_returns_author_command_containing_two_authors_with_one_frequencyDTO_with_frequency_of_one(){
        //Arrange
        var commitDateTime1 = DateTimeOffset.Now;
        var author1 = new Signature("mlth", "mlth@itu.dk", commitDateTime1);
        var commitDateTime2 = DateTimeOffset.Now;
        var author2 = new Signature("aarv", "aarv@itu.dk", commitDateTime2);
        var command = (AuthorCommand)Factory.getCommand("author");

        //Act
        repo.Commit("First commit", author1, author1, new CommitOptions(){ AllowEmptyCommit = true });
        repo.Commit("Second commit", author2, author2, new CommitOptions(){ AllowEmptyCommit = true });
        command.execute(repo);

        //Assert
        Assert.Equal("aarv", command.authors[0].Author);
        Assert.Equal("mlth", command.authors[1].Author);
        Assert.Equal(1, command.authors[0].frequencies[0].frequency);
        Assert.Equal(1, command.authors[1].frequencies[0].frequency);
        Assert.Equal(commitDateTime1.Date.ToShortDateString(), command.authors[0].frequencies[0].dateTime);
        Assert.Equal(commitDateTime2.Date.ToShortDateString(), command.authors[1].frequencies[0].dateTime);
        Assert.Equal(2, command.authors.Count);
    }

    //AnDerS
    //Test AuthorDTO with multiple commits by one author
    [Fact]
    public void Multiple_commits_by_one_author_returns_author_command_containing_one_author_with_one_frequencyDTO_with_frequency_of_two(){
        //Arrange
        var commitDateTime1 = DateTimeOffset.Now;
        var author1 = new Signature("mlth", "mlth@itu.dk", commitDateTime1);
        var command = (AuthorCommand)Factory.getCommand("author");

        //Act
        repo.Commit("First commit", author1, author1, new CommitOptions(){ AllowEmptyCommit = true });
        repo.Commit("Second commit", author1, author1, new CommitOptions(){ AllowEmptyCommit = true });
        command.execute(repo);

        //Assert
        Assert.Equal("mlth", command.authors[0].Author);
        Assert.Equal(2, command.authors[0].frequencies[0].frequency);
        Assert.Equal(commitDateTime1.Date.ToShortDateString(), command.authors[0].frequencies[0].dateTime);
        Assert.Equal(1, command.authors.Count);
    }

    //Test AuthorDTO with multiple commits by multiple authors
    //Test AuthorDTO with one commit by multiple authors
    [Fact]
    public void Multiple_commits_by_multiple_authors_returns_author_command_containing_two_authors_with_one_frequencyDTO_with_frequency_of_two(){
        //Arrange
        var commitDateTime1 = DateTimeOffset.Now;
        var author1 = new Signature("mlth", "mlth@itu.dk", commitDateTime1);
        var commitDateTime2 = DateTimeOffset.Now;
        var author2 = new Signature("aarv", "aarv@itu.dk", commitDateTime2);
        var command = (AuthorCommand)Factory.getCommand("author");

        //Act
        repo.Commit("First commit", author1, author1, new CommitOptions(){ AllowEmptyCommit = true });
        repo.Commit("Second commit", author1, author1, new CommitOptions(){ AllowEmptyCommit = true });
        repo.Commit("Third commit", author2, author2, new CommitOptions(){ AllowEmptyCommit = true });
        repo.Commit("Fourth commit", author2, author2, new CommitOptions(){ AllowEmptyCommit = true });
        command.execute(repo);

        //Assert
        Assert.Equal("aarv", command.authors[0].Author);
        Assert.Equal("mlth", command.authors[1].Author);
        Assert.Equal(2, command.authors[0].frequencies[0].frequency);
        Assert.Equal(2, command.authors[1].frequencies[0].frequency);
        Assert.Equal(commitDateTime1.Date.ToShortDateString(), command.authors[0].frequencies[0].dateTime);
        Assert.Equal(commitDateTime2.Date.ToShortDateString(), command.authors[1].frequencies[0].dateTime);
        Assert.Equal(2, command.authors.Count);
    }
    

    //Test AuthorDTO with no commits
    [Fact]
    public void No_commits_by_zero_authors_returns_author_command_containing_zero_authors_with_zero_frequencyDTO(){
        //Arrange
        var command = (AuthorCommand)Factory.getCommand("author");

        //Act
        command.execute(repo);

        //Assert
        Assert.Equal(0, command.authors.Count);
    }

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
