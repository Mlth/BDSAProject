using LibGit2Sharp;
using Octokit;
namespace GitInsight;

public class WebProgram
{
    string githubApiKey;

    public WebProgram(string githubApiKey){
        this.githubApiKey = githubApiKey;
    }
    public async Task<string> GetAnalysisJsonStringAsync(string repositoryLink, string repository_name, string inputCommand)
    {
        var productInformation = new ProductHeaderValue("luel");
        var credentials = new Octokit.Credentials(githubApiKey);
        var client = new GitHubClient(productInformation) { Credentials = credentials };

        IReadOnlyList<Octokit.Repository> allForks = await client.Repository.Forks.GetAll(
            "Mlth", "BDSAProject");
        /*foreach (Octokit.Repository fork in allForks){
            Console.WriteLine(fork.CloneUrl);
        }*/
        
        var repositories = Directory.GetParent(Directory.GetCurrentDirectory()) + "/Repositories/";
        foreach(string s in Directory.GetDirectories(Directory.GetCurrentDirectory(), repository_name)){
            Console.WriteLine(s);
        }
        var repositoryLocation = repositories + repository_name;
        if(Directory.GetDirectories(repositories, repository_name).Length < 1){
            cloneRepository(repositoryLink, repositoryLocation);
        } else {
            pullRepository(repositoryLocation);
        }

        RepositoryContextFactory factory = new RepositoryContextFactory();
        RepositoryContext context = factory.CreateDbContext(new string[] {});
        var repository = new LibGit2Sharp.Repository(repositoryLocation);
        var command = Factory.getCommand(inputCommand);
        command.template(repository, context);
        var analysis = command.getAnalysis();
        var jsonString = analysis.analyze();
        repository.Dispose();
        //deleteDirectory(repositoryLocation);
        return jsonString;
    }

    public static void cloneRepository(string repositories, string newDir){
        LibGit2Sharp.Repository.Clone(repositories, newDir);
    }

    public static void pullRepository(string repositories){
        var repository = new LibGit2Sharp.Repository(repositories);
        var signature = new LibGit2Sharp.Signature("Lukas", "luel@itu.dk", DateTime.Now);
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
            var fileInfo = new FileInfo(@fileName);
            fileInfo.Attributes = FileAttributes.Normal;
            fileInfo.Delete();
        }
        var dir = new DirectoryInfo(path);
        dir.Delete(true);
    }
}
