namespace GitInsight;

public class Program
{
    public static void Main(string[] args)
    {
        RepositoryContextFactory factory = new RepositoryContextFactory();
        RepositoryContext context = factory.CreateDbContext(args);
        var repository = new Repository(args[0]);
        var command = Factory.getCommand(args[1]);
        command.template(repository, context);
        var visualizer = command.getVisualizer();
        visualizer.visualize();
    }
}
