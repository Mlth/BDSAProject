namespace GitInsight.Entities;
using GitInsight.Core;

public record DBRepository
{
    public int Id { get; set; }

    public int state { get; set; }
}