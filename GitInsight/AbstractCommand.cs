using System.Text.RegularExpressions;
using GitInsight.Entities.DTOS;

namespace GitInsight;
public abstract class AbstractCommand{
    public String repoID {get; set;}
    public void template(Repository repo, RepositoryContext context){
        var repoName = repo.Network.Remotes.First().Url;
        var pattern = @"github.com[\/|:](?<repo>(.+))";
        repoID = Regex.Match(repoName, pattern).Groups["repo"].Value;
        if (needsUpdate(repo, context)){
            analyseRepoAndSave(repo, context);
        }
        else{
            fetchData(context);
        }
    }

    protected abstract void fetchData(RepositoryContext context);
    protected abstract void analyseRepoAndSave(Repository repo, RepositoryContext context);
    protected bool needsUpdate(Repository repo, RepositoryContext context){
        
        var repository = new DBRepoRepository(context);
        var entity = repository.Read(new DBRepositoryDTO{name = repoID});
        if (entity is not null){
            if (repo.Commits.Last().Id.RawId.ToString() == entity.state){
                return false;
            }
            return true;
        }
        repository.Create(new DBRepositoryDTO{name = repoID, state = repo.Commits.Last().Id.RawId.ToString()});
        return true;
        
    }

    public abstract IVisualizer getVisualizer();

    public abstract void testLogicWithoutContext(Repository repo);
}
