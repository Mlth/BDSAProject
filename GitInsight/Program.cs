namespace GitInsight;
using GitInsight.Entities;
using LibGit2Sharp;

public class Program
{
    public static void Main(string[] args)
    {
        var repository = new Repository(args[0]);
        var command = Factory.getCommand(args[1]);
        command.execute(repository);
        var visualizer = command.getVisualizer();
        visualizer.visualize();
    }
}
