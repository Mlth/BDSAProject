namespace GitInsight;

public class Program
{
    public static void Main(string[] args)
    {
        RepositoryContextFactory factory = new RepositoryContextFactory();
        RepositoryContext context = factory.CreateDbContext(args);
        var repo = new Repository(args[0]);
        var command = Factory.getCommandAndIncjectDependencies(args[1], repo, context);
        command.processRepo();
        var visualizer = command.getVisualizer();
        visualizer.visualize();
    }
}
