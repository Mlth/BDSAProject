using System.Text.Json;

namespace GitInsight;

public class AuthorObject
{
    public string? Author { get; set; }
    public List<string>? Frequencies { get; set; }
}

public class AuthorAnalysis : IAnalysis
{
    IEnumerable<AuthorDTO> dtos;

    public AuthorAnalysis(IEnumerable<AuthorDTO> dtos)
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