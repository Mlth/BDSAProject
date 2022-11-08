namespace GitInsight;

public class FrequencyAnalysis : IAnalysis
{
    List<FrequencyDTO> dtos;
    List<string> analysis;

    public FrequencyAnalysis(List<FrequencyDTO> dtos)
    {
        this.dtos = dtos;
        analysis = new List<string>();
    }

    public List<string> analyze()
    {
        foreach (FrequencyDTO dto in dtos)
        {
            analysis.Add(dto.frequency + " " + dto.dateTime);
        }
        return analysis;
    }
}