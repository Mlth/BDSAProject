namespace GitInsight.Tests;
using LibGit2Sharp;
using System.IO;
using GitInsight;
using GitInsight.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public class GitInsightCMDTest : IDisposable
{
    private readonly RepositoryContext context;
    private readonly DBRepoRepository repoRepository;

    Repository repo;
    string path = @".\test-repo\";

    public GitInsightCMDTest(){
        Repository.Init(path);
        repo = new Repository(path);
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<RepositoryContext>();
        builder.UseSqlite(connection);
        context = new RepositoryContext(builder.Options);
        context.Database.EnsureCreated();
        repoRepository = new DBRepoRepository(context);
        //Adding first DBRepo to DB
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
    public void no_commits_returns_throws_NoCommitsException(){
        //Arrange
        var command = Factory.getCommand("frequency");

        command.Invoking(y => y.template(repo, context))
        .Should().Throw<NoCommitsException>()
        .WithMessage("The repository contains no commits");
    }

    [Fact]
    public void One_commit_by_one_author_returns_frequency_command_containing_one_frequencyDTO(){
        //Arrange
        var commitDateTime1 = DateTimeOffset.Now;
        var author1 = new Signature("mlth", "mlth@itu.dk", commitDateTime1);
        var command = (FrequencyCommand)Factory.getCommand("frequency");

        //Act
        repo.Commit("First commit", author1, author1, new CommitOptions(){ AllowEmptyCommit = true });
        command.template(repo, context);

        //Assert
        Assert.Equal(1, command.frequencies.ToArray()[0].frequency);
        Assert.Equal(commitDateTime1.Date.ToShortDateString(), command.frequencies.ToArray()[0].date.ToShortDateString());
        Assert.Equal(1, command.frequencies.Count());
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
        command.template(repo, context);

        //Assert
        Assert.Equal(2, command.frequencies.ToArray()[0].frequency);
        Assert.Equal(commitDateTime1.Date.ToShortDateString(), command.frequencies.ToArray()[0].date.ToShortDateString());
        Assert.Equal(1, command.frequencies.Count());
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
        command.template(repo, context);

        //Assert
        Assert.Equal(2, command.frequencies.ToArray()[0].frequency);
        Assert.Equal(commitDateTime1.Date.ToShortDateString(), command.frequencies.ToArray()[0].date.ToShortDateString());
        Assert.Equal(1, command.frequencies.Count());
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
        command.template(repo, context);

        //Assert
        Assert.Equal(4, command.frequencies.ToArray()[0].frequency);
        Assert.Equal(commitDateTime1.Date.ToShortDateString(), command.frequencies.ToArray()[0].date.ToShortDateString());
        Assert.Equal(1, command.frequencies.Count());
    }

    [Fact]
    public void One_commit_by_one_author_returns_author_command_containing_one_author_with_one_frequencyDTO_with_frequency_of_one(){
        //Arrange
        var commitDateTime1 = DateTimeOffset.Now;
        var author1 = new Signature("mlth", "mlth@itu.dk", commitDateTime1);
        var command = (AuthorCommand)Factory.getCommand("author");

        //Act
        repo.Commit("First commit", author1, author1, new CommitOptions(){ AllowEmptyCommit = true });
        command.template(repo, context);

        //Assert
        Assert.Equal("mlth", command.authors.ToArray()[0].author);
        Assert.Equal(1, command.authors.ToArray()[0].frequencies.ToArray()[0].frequency);
        Assert.Equal(commitDateTime1.Date.ToShortDateString(), command.authors.ToArray()[0].frequencies.ToArray()[0].date.ToShortDateString());
        Assert.Equal(1, command.authors.Count());
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
        command.template(repo, context);

        //Assert
        Assert.Equal("aarv", command.authors.ToArray()[0].author);
        Assert.Equal("mlth", command.authors.ToArray()[1].author);
        Assert.Equal(1, command.authors.ToArray()[0].frequencies.ToArray()[0].frequency);
        Assert.Equal(1, command.authors.ToArray()[1].frequencies.ToArray()[0].frequency);
        Assert.Equal(commitDateTime1.Date.ToShortDateString(), command.authors.ToArray()[0].frequencies.ToArray()[0].date.ToShortDateString());
        Assert.Equal(commitDateTime2.Date.ToShortDateString(), command.authors.ToArray()[1].frequencies.ToArray()[0].date.ToShortDateString());
        Assert.Equal(2, command.authors.Count());
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
        command.template(repo, context);

        //Assert
        Assert.Equal("mlth", command.authors.ToArray()[0].author);
        Assert.Equal(2, command.authors.ToArray()[0].frequencies.ToArray()[0].frequency);
        Assert.Equal(commitDateTime1.Date.ToShortDateString(), command.authors.ToArray()[0].frequencies.ToArray()[0].date.ToShortDateString());
        Assert.Equal(1, command.authors.Count());
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
        command.template(repo, context);

        //Assert
        Assert.Equal("aarv", command.authors.ToArray()[0].author);
        Assert.Equal("mlth", command.authors.ToArray()[1].author);
        Assert.Equal(2, command.authors.ToArray()[0].frequencies.ToArray()[0].frequency);
        Assert.Equal(2, command.authors.ToArray()[1].frequencies.ToArray()[0].frequency);
        Assert.Equal(commitDateTime1.Date.ToShortDateString(), command.authors.ToArray()[0].frequencies.ToArray()[0].date.ToShortDateString());
        Assert.Equal(commitDateTime2.Date.ToShortDateString(), command.authors.ToArray()[1].frequencies.ToArray()[0].date.ToShortDateString());
        Assert.Equal(2, command.authors.Count());
    }
    

    //Test AuthorDTO with no commits
    [Fact]
    public void No_commits_by_zero_authors_returns_author_command_containing_zero_authors_with_zero_frequencyDTO(){
        //Arrange
        var command = (AuthorCommand)Factory.getCommand("author");

        //Act
        command.Invoking(y => y.template(repo, context))
        .Should().Throw<NoCommitsException>()
        .WithMessage("The repository contains no commits");
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