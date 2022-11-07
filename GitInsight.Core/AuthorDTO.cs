namespace GitInsight.Core;
using GitInsight.Core;
public record AuthorDTO{
    public string author {get; set;}
    public IEnumerable<FrequencyDTO> frequencies {get; set;} = new List<FrequencyDTO>();
}