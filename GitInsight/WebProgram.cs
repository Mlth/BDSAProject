namespace GitInsight;
using LibGit2Sharp;

public class WebProgram
{
    public List<string> GetAnalysisList(string repositoryPath, string inputCommand)
    {
        var repository = new Repository(repositoryPath);
        var command = Factory.getCommand(inputCommand);
        command.execute(repository);
        var analysis = command.getAnalysis();
        var analysisList = analysis.analyze();
        return analysisList;
    }
}
