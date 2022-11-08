namespace GitInsight;

public class AuthorAnalysis : IAnalysis
{
    List<AuthorDTO> dtos;
    List<string> analysis;

    public AuthorAnalysis(List<AuthorDTO> dtos)
    {
        this.dtos = dtos;
        analysis = new List<string>();
    }

    public List<string> analyze()
    {
        foreach (AuthorDTO dto in dtos)
        {
            analysis.Add(dto.Author);
            foreach (FrequencyDTO freq in dto.frequencies)
            {
                analysis.Add("----" + freq.frequency + " " + freq.dateTime);
            }
        }
        return analysis;
    }
}