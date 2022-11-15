namespace GitInsight;
using LibGit2Sharp;
using GitInsight.Entities.DTOS;
using GitInsight.Entities;

public class AuthorCommand : AbstractCommand {

    public IEnumerable<AuthorDTO> authors {get; set;} = new List<AuthorDTO>();

    public override void fetchData(RepositoryContext context)
    {
        var repository = new DBRepoRepository(context);
        IEnumerable<DBCommit> commits = repository.ReadAllCommits(new DBRepositoryDTO{name = repoID});
        authors = from c in commits
                    group c by c.author into group1
                    select new AuthorDTO{author = group1.Key, frequencies = new List<FrequencyDTO>().Concat(
                        from c in group1
                        group c by c.date.Date into group2
                        select new FrequencyDTO {date = group2.Key, frequency = group2.Count()})};
    }

    public override IVisualizer getVisualizer()
    {
        return new AuthorVisualizer(authors);
    }

    public override IAnalysis getAnalysis()
    {
        return new AuthorAnalysis(authors);
    }
}