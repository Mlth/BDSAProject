namespace GitInsight.Entities.Tests;
using LibGit2Sharp;

public class DBCommitRepositoryTests : IDisposable
{
    private readonly RepositoryContext context;
    private readonly DBRepoRepository repoRepository;

    Repository repo;
    string path = @".github.com\test-repo\";

    public DBCommitRepositoryTests(){
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
    public void One_commit_by_one_author_returns_frequency_command_containing_one_frequencyDTO(){
        //Arrange
        var commitDateTime1 = DateTimeOffset.Now;
        var author1 = new Signature("mlth", "mlth@itu.dk", commitDateTime1);
        var command = (FrequencyCommand)Factory.getCommand("frequency");

        //Act
        repo.Commit("First commit", author1, author1, new CommitOptions(){ AllowEmptyCommit = true });
        Console.WriteLine(repo.Commits.FirstOrDefault().Author.Name);
        command.template(repo, context);

        //Assert
        Assert.Equal(1, command.frequencies.ToArray()[0].frequency);
        Assert.Equal(commitDateTime1.Date.ToShortDateString(), command.frequencies.ToArray()[0].date.ToShortDateString());
        Assert.Equal(1, command.frequencies.Count());
    }
    



    //opret repository
    //opret find
    //update repository

    public void Dispose() {
        repo.Dispose();
        DeleteReadOnlyDirectory(path);
        context.Dispose();
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