using LibGit2Sharp;

namespace GitInsight;

public interface Command
{
    void execute(Repository repo);
    IVisualizer getVisualizer();
    IAnalysis getAnalysis();
}