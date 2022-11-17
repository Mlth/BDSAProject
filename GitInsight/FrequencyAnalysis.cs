using System.Text.Json;

namespace GitInsight;

public class FrequencyObject
{
    public List<string>? Frequencies { get; set; }
}

public class FrequencyAnalysis : IAnalysis
{
    IEnumerable<FrequencyDTO> dtos;

    public FrequencyAnalysis(IEnumerable<FrequencyDTO> dtos)
    {
        this.dtos = dtos;
    }

    public string analyze()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var jsonString = JsonSerializer.Serialize(dtos, options);
        return jsonString;
    }
}