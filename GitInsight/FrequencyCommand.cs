namespace GitInsight;
using LibGit2Sharp;
using GitInsight.Entities.DTOS;
using GitInsight.Entities;

public class FrequencyCommand : AbstractCommand {

    IEnumerable<FrequencyDTO> commitData = new List<FrequencyDTO>();

    public override IVisualizer getVisualizer(){
        return new FrequencyVisualizer(commitData);
    }

    protected override void analyseRepoAndSave(Repository repo, RepositoryContext context)
    {
        IEnumerable<DBCommit> commits = from c in repo.Commits
                        select new DBCommit{author = c.Author.Name, date = c.Author.When.Date, repo = new DBRepository{name = repoID, state = repo.Commits.Last().Id.RawId.ToString()}};
        saveToDatabase(context, commits);
    }

    protected override void fetchData(RepositoryContext context)
    {
        DBCommitRepository repository = new DBCommitRepository(context);
        IEnumerable<DBCommit> commits = repository.ReadAll();
        commitData = from c in commits
                    group c by c.date into group1
                    select new FrequencyDTO{date = group1.Key, frequency = group1.Count()};
    }

    public override void testLogicWithoutContext(Repository repo){
        IEnumerable<DBCommit> commits = from c in repo.Commits
                        select new DBCommit{author = c.Author.Name, date = c.Author.When.Date, repo = new DBRepository{name = repoID, state = repo.Commits.Last().Id.RawId.ToString()}};
        commitData = from c in commits
                    group c by c.date into group1
                    select new FrequencyDTO{date = group1.Key, frequency = group1.Count()};
    }


    protected void saveToDatabase(RepositoryContext context, IEnumerable<DBCommit> commits)
    {
        DBCommitRepository repository = new DBCommitRepository(context);
        foreach (var commit in commits){
            repository.CreateOrUpdate(commit);
        }
    }
}
