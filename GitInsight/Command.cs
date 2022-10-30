namespace GitInsight;
using LibGit2Sharp;
interface Command{
    void execute(Repository repo);
    IVisualizer getVisualizer();
}