namespace GitInsight.Entities;
using GitInsight.Core;

public record RepositoryCommitData
{
    public int Id { get; set; }

    public string? author { get; set; }

    public List<FrequencyDTO>? frequencies { get; set; }

    public DateTime dateTime { get; set; }
}