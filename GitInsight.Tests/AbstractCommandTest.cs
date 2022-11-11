/* namespace GitInsight.Tests;
using LibGit2Sharp;
using System.IO;
using GitInsight;
using GitInsight.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public class AbstractCommandTest : IDisposable
{
    private readonly RepositoryContext context;
    private readonly DBRepoRepository repoRepository;

    Repository repo;
    string path = @".\test-repo\";

    public AbstractCommandTest(){
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
    public void needsUpdate_should_return_true_after_new_commit_to_repo(){
        var commitDateTime1 = DateTimeOffset.Now;
        var author1 = new Signature("mlth", "mlth@itu.dk", commitDateTime1);
        var commitDateTime2 = DateTimeOffset.Now;
        var author2 = new Signature("aarv", "aarv@itu.dk", commitDateTime2);
        var firstCommand = new FrequencyCommand();

        //Act
        repo.Commit("First commit", author1, author1, new CommitOptions(){ AllowEmptyCommit = true });
        firstCommand.template(repo, context);
        repo.Commit("Second commit", author2, author2, new CommitOptions(){ AllowEmptyCommit = true });
        var secondCommand = new FrequencyCommand();
        secondCommand.repoID = firstCommand.repoID;
        secondCommand.needsUpdate(repo, context).Should().BeTrue();
    }

    [Fact]
    public void needsUpdate_should_return_false_after_no_new_commit_to_repo(){
        var commitDateTime1 = DateTimeOffset.Now;
        var author1 = new Signature("mlth", "mlth@itu.dk", commitDateTime1);
        var commitDateTime2 = DateTimeOffset.Now;
        var author2 = new Signature("aarv", "aarv@itu.dk", commitDateTime2);
        var firstCommand = new FrequencyCommand();

        //Act
        repo.Commit("First commit", author1, author1, new CommitOptions(){ AllowEmptyCommit = true });
        firstCommand.template(repo, context);
        var secondCommand = new FrequencyCommand();
        secondCommand.repoID = firstCommand.repoID;
        secondCommand.needsUpdate(repo, context).Should().BeFalse();
    }

    [Fact]
    public void getDBCommits_should_return_two_DBCommits(){
        var commitDateTime1 = DateTimeOffset.Now;
        var author1 = new Signature("mlth", "mlth@itu.dk", commitDateTime1);
        var commitDateTime2 = DateTimeOffset.Now;
        var author2 = new Signature("aarv", "aarv@itu.dk", commitDateTime2);
        var command = new FrequencyCommand();
        repo.Commit("First commit", author1, author1, new CommitOptions(){ AllowEmptyCommit = true });
        repo.Commit("Second commit", author2, author2, new CommitOptions(){ AllowEmptyCommit = true });
        var dbcommits = command.getDBCommits(repo);
        dbcommits.Should().BeEquivalentTo(new List<DBCommit>(){
            new DBCommit{author = "mlth", date = commitDateTime1.Date, repo = new DBRepository{
                                                                            name = null, 
                                                                            state = repo.Commits.First().Sha}},
            new DBCommit{author = "aarv", date = commitDateTime2.Date, repo = new DBRepository{
                                                                            name = null, 
                                                                            state = repo.Commits.First().Sha}}
            });
    }

    [Fact]
    public void needsUpdate_should_return_false_if_no_repo_exists(){
        var commitDateTime1 = DateTimeOffset.Now;
        var author1 = new Signature("mlth", "mlth@itu.dk", commitDateTime1);
        var firstCommand = new FrequencyCommand();

        //Act
        repo.Commit("First commit", author1, author1, new CommitOptions(){ AllowEmptyCommit = true });
        firstCommand.repoID = repo.Commits.Last().Sha;
        firstCommand.needsUpdate(repo, context).Should().BeFalse();
    }
    
    public void Dispose(){
        repo.Dispose();
        DeleteReadOnlyDirectory(path);
    }

    [Fact]
    public void getDBCommits_should_return_zero_DBCommits(){
        var command = new FrequencyCommand();
        var dbcommits = command.getDBCommits(repo);
        dbcommits.Should().BeEmpty();
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
} */