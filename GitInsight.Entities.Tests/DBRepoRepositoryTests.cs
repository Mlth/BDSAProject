namespace GitInsight.Entities.Tests;

public class DBRepoRepositoryTests : IDisposable
{
    private readonly RepositoryContext context;
    private readonly DBRepoRepository repository;

    public DBRepoRepositoryTests(){
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<RepositoryContext>();
        builder.UseSqlite(connection);
        context = new RepositoryContext(builder.Options);
        context.Database.EnsureCreated();

        repository = new DBRepoRepository(context);
        //Adding first DBRepo to DB
        repository.Create(new DBRepositoryDTO{name = "Mlth/RepositoryName", state = "state"});
        context.SaveChanges();
    }

    [Fact]
    public void CreateNewRepoReturnsCreatedResponse(){
        //Arrange
        var repositoryDTO = new DBRepositoryDTO{name = "Mlth/SecondRepositoryName", state = "state2"};

        //Act
        var response = repository.Create(repositoryDTO);

        //Assert
        Assert.Equal(Response.Created, response);
    }

    [Fact]
    public void CreateTwoIdenticalReposReturnsConflict(){
        var conflictRepositoryDTO = new DBRepositoryDTO{name = "Mlth/RepositoryName", state = "state"};

        Response secondResponse = repository.Create(conflictRepositoryDTO);

        Assert.Equal(Response.Conflict, secondResponse);
    }

    [Fact]
    public void FindValidRepoReturnsCorrectRepo(){
        DBRepository dto = repository.Read(new DBRepositoryDTO{name = "Mlth/RepositoryName"});

        Assert.Equal("Mlth/RepositoryName", dto.name);
        Assert.Equal("state", dto.state);
    }

    [Fact]
    public void FindInvalidRepoReturnsNull(){
        DBRepository dto = repository.Read(new DBRepositoryDTO{name = "Mlth/InvalidRepositoryName"});

        Assert.Null(dto);
    }
    
    [Fact]
    public void UpdateValidRepoReturnsUpdated(){
        DBRepositoryDTO newDTO = new DBRepositoryDTO{name = "Mlth/RepositoryName", state= "newState"};
        (Response response, DBRepositoryDTO updatedDTO) = repository.Update(newDTO);

        Assert.Equal(Response.Updated, response);
        Assert.Equal("newState", newDTO.state);
    }
    
    [Fact]
    public void UpdateInvalidRepoReturnsUpdated(){
        DBRepositoryDTO newDTO = new DBRepositoryDTO{name = "Mlth/InvalidRepositoryName", state= "newState"};
        (Response response, DBRepositoryDTO updatedDTO) = repository.Update(newDTO);

        Assert.Equal(Response.NotFound, response);
        Assert.Null(updatedDTO);
    }

    public void Dispose() {
        context.Dispose();
    }

    //Test that correct commits are retrieved when reading, updating, and creating
}