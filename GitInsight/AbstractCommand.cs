using System.Text.RegularExpressions;

namespace GitInsight;
public abstract class AbstractCommand{
    public void template(Repository repo, RepositoryContext context){
        if (needsUpdate(repo, context)){
            analyseRepo(repo);
            saveToDatabase(context);
        }
        else{
            fetchData(context);
        }
    }

    protected abstract void saveToDatabase(RepositoryContext context);
    protected abstract void fetchData(RepositoryContext context);
    protected abstract void analyseRepo(Repository repo);
    protected bool needsUpdate(Repository repo, RepositoryContext context){
        var repoName = repo.Network.Remotes.First().Url;
        var pattern = @"github.com[\/|:](?<repo>(.+))";
        var match = Regex.Match(repoName, pattern).Groups["repo"].Value;
        var repository = new DBRepoRepository(context);
        if (repo.Commits.Last().Id.RawId.ToString() == repository.Find(match).state){
            return false;
        }
        return true;
    }

    public abstract IVisualizer getVisualizer();
}
