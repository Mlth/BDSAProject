
namespace GitInsight.Core;

public record DBCommitDTO{
    public int frequency {get; set;}
    public string? Author {get; set;}

    public int repoID {get; set;}

    public DateTime date {get; set;}
}