namespace GitInsight;


public class Program
{
    public static void Main(string[] args)
    {
        RepositoryContextFactory factory = new RepositoryContextFactory();
        RepositoryContext context = factory.CreateDbContext(args);
        Console.WriteLine($"Database path: {context.DbPath}.");
        context.CommitData.Add(new RepositoryCommitData{Id = 1, author = "MCAndersYo", frequencies = new List<Core.FrequencyDTO>{}, dateTime = new DateTime(2022, 11, 3)});
        var commitData = context.CommitData
            .OrderBy(c => c.Id)
            .First();
        Console.WriteLine(commitData.author);
        var repository = new Repository(args[0]);
        var command = Factory.getCommand(args[1]);
        command.execute(repository);
        var visualizer = command.getVisualizer();
        visualizer.visualize();
    }
}
