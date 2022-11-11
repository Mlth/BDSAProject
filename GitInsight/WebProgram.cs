using LibGit2Sharp;

namespace GitInsight;

public class WebProgram
{
    public string GetAnalysisJsonString(string repositoryPath, string inputCommand)
    {
        var repository = new Repository(repositoryPath);
        var command = Factory.getCommand(inputCommand);
        command.execute(repository);
        var analysis = command.getAnalysis();
        var jsonString = analysis.analyze();
        repository.Dispose();
        return jsonString;
    }
}
