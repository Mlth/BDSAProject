namespace GitInsight.Entities;
using GitInsight.Core;

public record DBRepository
{
    public string Id { get; set; }

    public string state { get; set; }

    public IEnumerable<DBCommit> commits {get; set;}
}