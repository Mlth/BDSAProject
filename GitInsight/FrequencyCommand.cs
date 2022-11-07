namespace GitInsight;
using LibGit2Sharp;
using GitInsight.Core;
using GitInsight.Entities;

public class FrequencyCommand : AbstractCommand {

    IEnumerable<DBCommit> commitData = new List<DBCommit>();

    public override IVisualizer getVisualizer(){
        return new FrequencyVisualizer((from c in commitData
                group c by c.date into group1
                select new FrequencyDTO {date = group1.Key, frequency = group1.Sum(x => x.frequency)}).ToList());
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
