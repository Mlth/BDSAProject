namespace GitInsight;
using LibGit2Sharp;
using GitInsight.Core;

public class AuthorCommand : AbstractCommand {

    IEnumerable<DBCommit> commitData {get; set;} = new List<DBCommit>();

    public override IVisualizer getVisualizer(){

        return new AuthorVisualizer(from c in commitData
                    group c by c.author into group1
                    select new AuthorDTO{author = group1.Key, frequencies = new List<FrequencyDTO>().Concat(
                        from c in group1
                        group c by c.date into group2
                        select new FrequencyDTO {date = group2.Key, frequency = group2.Sum(x => x.frequency)})});
    }

    protected override void analyseRepo(Repository repo)
    {
        commitData = (from c in repo.Commits
                    group c by c.Author.Name into group1
                    from group2 in (
                        from d in group1
                        group d by d.Author.When.ToString("d")
                    )
                    from e in group2
                    select new DBCommit{author = e.Author.Name, date = e.Author.When.Date, frequency = group2.Count()}).DistinctBy(x => (x.date, x.author));
        foreach (var c in commitData){
            Console.WriteLine(c);
        }
    }

    protected override void fetchData(RepositoryContext context)
    {
        DBCommitRepository repository = new DBCommitRepository(context);
        commitData = repository.ReadAll();
    }

    protected override void saveToDatabase(RepositoryContext context)
    {
        DBCommitRepository repository = new DBCommitRepository(context);
        foreach (var commit in commitData){
            repository.CreateOrUpdate(commit);
        }
    }
}