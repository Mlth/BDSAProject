using System.Text.RegularExpressions;
using GitInsight.Entities.DTOS;

namespace GitInsight;
public abstract class AbstractCommand{
    public String repoID {get; set;}
    public void template(Repository repo, RepositoryContext context){
        if (!repo.Commits.Any()){
            throw new NoCommitsException("The repository contains no commits");
        }
        repoID = repo.Commits.LastOrDefault().Sha;
        //analyseRepoAndUpdate(repo, context);
        if (needsUpdate(repo, context)){
            analyseRepoAndUpdate(repo, context);
            fetchData(context);
        }
        else {
            fetchData(context);
        }
    }

    public ICollection<DBCommit> getDBCommits(Repository repo){
        return (from c in repo.Commits
                        select new DBCommit{Id = c.Sha, author = c.Author.Name, date = c.Author.When.DateTime, repo = new DBRepository{name = repoID, state = repo.Commits.First().Sha}}).ToList();
    }

    public ICollection<DBCommit> getUpdatedDBCommits(Repository repo, RepositoryContext context){
        DBRepoRepository repository = new DBRepoRepository(context);
        var lastCommit = repository.ReadLastCommit(new DBRepositoryDTO{name = repoID});
        return (from c in repo.Commits
                        where c.Author.When > lastCommit.date
                        select new DBCommit{Id = c.Sha, author = c.Author.Name, date = c.Author.When.DateTime, repo = new DBRepository{name = repoID, state = repo.Commits.First().Sha}}).ToList();
    }

    public void analyseRepoAndUpdate(Repository repo, RepositoryContext context)
    {
        DBRepoRepository repository = new DBRepoRepository(context);
        repository.Update(new DBRepositoryDTO{name = repoID, state = repo.Commits.First().Sha, commits = getUpdatedDBCommits(repo, context)});
        //repository.Update(new DBRepositoryDTO{name = repoID, state = repo.Commits.First().Sha, commits = getDBCommits(repo)});
    }
    public abstract void fetchData(RepositoryContext context);
    public bool needsUpdate(Repository repo, RepositoryContext context){
        
        var repository = new DBRepoRepository(context);
        var entity = repository.Read(new DBRepositoryDTO{name = repoID});
        if (entity is not null){
            if (repo.Commits.First().Sha == entity.state){
                return false;
            }
            return true;
        }
        repository.Create(new DBRepositoryDTO{name = repoID, state = repo.Commits.First().Sha, commits = getDBCommits(repo)});
        return false;
    }

    public abstract IVisualizer getVisualizer();

    public abstract IAnalysis getAnalysis();
}
