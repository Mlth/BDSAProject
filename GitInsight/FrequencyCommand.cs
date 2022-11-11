namespace GitInsight;
using LibGit2Sharp;
using GitInsight.Entities.DTOS;
using GitInsight.Entities;

public class FrequencyCommand : AbstractCommand {

    public ICollection<FrequencyDTO> frequencies = new List<FrequencyDTO>();

    public override IVisualizer getVisualizer(){
        return new FrequencyVisualizer(frequencies);
    }

    public override void fetchData(RepositoryContext context)
    {
        var repository = new DBRepoRepository(context);
        IEnumerable<DBCommit> commits = repository.ReadAllCommits(new DBRepositoryDTO{name = repoID});
        frequencies = (from c in commits
                    group c by c.date into group1
                    select new FrequencyDTO{date = group1.Key, frequency = group1.Count()}).ToList();
    }

    public override void execute(Repository repo){
        ICollection<DBCommit> commits = getDBCommits(repo);
        frequencies = (from c in commits
                    group c by c.date into group1
                    select new FrequencyDTO{date = group1.Key, frequency = group1.Count()}).ToList();
    }
}
