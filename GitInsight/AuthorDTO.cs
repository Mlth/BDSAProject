namespace GitInsight;

public record AuthorDTO
{
    public string author;
    public IEnumerable<FrequencyDTO> frequencies {get; set;} = new List<FrequencyDTO>();
}