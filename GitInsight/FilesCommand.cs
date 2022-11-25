namespace GitInsight;
using LibGit2Sharp;
using GitInsight.Entities.DTOS;
using GitInsight.Entities;
using System.Text.Json;
using System.Text.Encodings.Web;

public class FilesCommand : AbstractCommand {

    public Dictionary<string,IEnumerable<String>> commitWhenAndWhat {get; set;} = new Dictionary<string, IEnumerable<string>>();

    public FilesCommand(Repository repo, RepositoryContext context) : base(repo, context){
        
    }

    public void CompareTrees(LibGit2Sharp.Commit c, LibGit2Sharp.Repository repo){
        if(c.Parents.ToArray().Length == 0){
            return;
        }
        List<string> changes = new List<string>();
        Tree commitTree = c.Tree; // Main Tree
        Commit parentCommit = c.Parents.First();
        Tree parentCommitTree = c.Parents.First().Tree; // Secondary Tree

        var patch = repo.Diff.Compare<Patch>(parentCommitTree, commitTree); // Difference

        foreach (var ptc in patch)
        {
            changes.Add(ptc.Status +" -> "+ptc.Path);
            Console.WriteLine(ptc.Status +" -> "+ptc.Path); // Status -> File Path
        }
        commitWhenAndWhat.Add(c.Author.When.DateTime.ToString(),changes);
        CompareTrees(parentCommit, repo);
    }

    public override void fetchData()
    {
        CompareTrees(libGitRepo.Head.Tip, libGitRepo);
    }

    public override string getJsonString()
    {
        var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
        var jsonString = JsonSerializer.Serialize(commitWhenAndWhat, options);

        return jsonString;
    }
}