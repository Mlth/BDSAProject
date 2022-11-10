using System.Text.Json;

namespace GitInsight;

public class FrequencyObject
{
    public List<string>? Frequencies { get; set; }
}

public class FrequencyAnalysis : IAnalysis
{
    List<FrequencyDTO> dtos;

    public FrequencyAnalysis(List<FrequencyDTO> dtos)
    {
        this.dtos = dtos;
    }

    public string analyze()
    {
        var tempList = new List<string>();

        foreach (FrequencyDTO dto in dtos)
        {
            tempList.Add(dto.frequency + " " + dto.dateTime);
        }

        var frequencyObject = new FrequencyObject
        {
            Frequencies = tempList
        };

        var options = new JsonSerializerOptions { WriteIndented = true };
        var jsonString = JsonSerializer.Serialize(frequencyObject, options);

        return jsonString;
    }
}