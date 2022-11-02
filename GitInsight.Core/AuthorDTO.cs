namespace GitInsight.Core;

public record AuthorDTO{
    public string? Author;
    public List<FrequencyDTO>? frequencies;
}