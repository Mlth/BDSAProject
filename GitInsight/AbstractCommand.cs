using System.Text.RegularExpressions;
using GitInsight.Entities.DTOS;

namespace GitInsight;
public abstract class AbstractCommand{
    public String repoID {get; set;}
    public void template(Repository repo, RepositoryContext context){
        repoID = repo.Commits.FirstOrDefault().Sha;
        if (needsUpdate(repo, context)){
            analyseRepoAndSave(repo, context);
        }
        else{
            fetchData(context);
        }
    }

    public ICollection<DBCommit> getDBCommits(Repository repo){
        ICollection<DBCommit> commits = (from c in repo.Commits
                        select new DBCommit{author = c.Author.Name, date = c.Author.When.Date, repo = new DBRepository{name = repoID, state = repo.Commits.Last().Id.RawId.ToString()}}).ToList();
        Console.WriteLine(commits.Count());
        return commits;
    }

    protected void analyseRepoAndSave(Repository repo, RepositoryContext context)
    {
        ICollection<DBCommit> commits = getDBCommits(repo);
        DBRepoRepository repository = new DBRepoRepository(context);
        repository.Update(new DBRepositoryDTO{name = repoID, state = repo.Commits.Last().Id.RawId.ToString(), commits = commits});
    }
    protected abstract void fetchData(RepositoryContext context);
    protected bool needsUpdate(Repository repo, RepositoryContext context){
        
        var repository = new DBRepoRepository(context);
        var entity = repository.Read(new DBRepositoryDTO{name = repoID});
        if (entity is not null){
            if (repo.Commits.Last().Id.RawId.ToString() == entity.state){
                return false;
            }
            return true;
        }
        repository.Create(new DBRepositoryDTO{name = repoID, state = repo.Commits.Last().Id.RawId.ToString(), commits = getDBCommits(repo)});
        return false;
    }

    public abstract IVisualizer getVisualizer();

    public abstract void execute(Repository repo);
}
