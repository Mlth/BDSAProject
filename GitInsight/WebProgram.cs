using LibGit2Sharp;
namespace GitInsight;

public class WebProgram
{
    public string GetAnalysisJsonString(string repositoryLink, string repository_name, string inputCommand)
    {
        var repositoryPath = Directory.GetParent(Directory.GetCurrentDirectory()) + "/Repositories";
        if(Directory.GetDirectories(Directory.GetCurrentDirectory(), repository_name).Length < 1){
            cloneRepository(repositoryLink, repositoryPath);
        } else {
            pullRepository(repositoryPath);
        }

        RepositoryContextFactory factory = new RepositoryContextFactory();
        RepositoryContext context = factory.CreateDbContext(new string[] {});
        var repository = new Repository(repositoryPath);
        var command = Factory.getCommand(inputCommand);
        command.template(repository, context);
        var analysis = command.getAnalysis();
        var jsonString = analysis.analyze();
        repository.Dispose();
        deleteDirectory(repositoryPath);
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
