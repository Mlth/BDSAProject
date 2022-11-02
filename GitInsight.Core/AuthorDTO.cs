namespace GitInsight.Core;

public record AuthorDTO{
    public int Id {get; set;}
    public string? Author;
    public List<FrequencyDTO>? frequencies;
}