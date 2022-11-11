using LibGit2Sharp;

namespace GitInsight;

public class WebProgram
{
    public string GetAnalysisJsonString(string repositoryPath, string inputCommand)
    {
        RepositoryContextFactory factory = new RepositoryContextFactory();
        RepositoryContext context = factory.CreateDbContext(new string[] {});
        var repository = new Repository(repositoryPath);
        var command = Factory.getCommand(inputCommand);
        command.template(repository, context);
        var analysis = command.getAnalysis();
        var jsonString = analysis.analyze();
        repository.Dispose();
        return jsonString;
    }
}
