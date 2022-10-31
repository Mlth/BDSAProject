namespace GitInsight;
using LibGit2Sharp;
public interface Command{
    void execute(Repository repo);
    IVisualizer getVisualizer();
}