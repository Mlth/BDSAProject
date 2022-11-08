namespace GitInsight.Entities;

public record DBRepository
{
    public string Id { get; set; }

    public string state { get; set; }

    public IEnumerable<DBCommit> commits {get; set;}
}