namespace GitInsight.WebApi;
using LibGit2Sharp;

public class TestGitFetcher : IGitFetcher {
    public void cloneRepository(string repositoryPath, string newDir){
        var repo = new Repository(Repository.Init(newDir));

        //Tilføjt nogle commits
        var commitDateTime1 = new DateTime(2022, 10, 10);
        var author1 = new Signature("mlth", "mlth@itu.dk", commitDateTime1);
        var commitDateTime2 = new DateTime(2022, 10, 10);
        var author2 = new Signature("aarv", "aarv@itu.dk", commitDateTime2);
        var commitDateTime3 = new DateTime(2022, 10, 11);
        var author3 = new Signature("aarv", "aarv@itu.dk", commitDateTime3);

        //Act
        repo.Commit("First commit", author1, author1, new CommitOptions(){ AllowEmptyCommit = true });
        repo.Commit("Second commit", author2, author2, new CommitOptions(){ AllowEmptyCommit = true });
        repo.Commit("Third commit", author3, author3, new CommitOptions(){ AllowEmptyCommit = true });
    }

    public void pullRepository(string repositoryPath){
        var repository = new Repository(repositoryPath);
        var signature = new Signature("Lukas", "luel@itu.dk", DateTime.Now);
        Commands.Pull(repository, signature, null);
    }
}