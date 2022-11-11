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
        var analysis = new List<AuthorObject>();

        foreach (AuthorDTO dto in dtos)
        {
            var tempList = new List<string>();
            string tempAuthor = dto.author;

            foreach (FrequencyDTO freq in dto.frequencies)
            {
                tempList.Add(freq.frequency + " " + freq.date);
            }

            var authorObject = new AuthorObject
            {
                Author = tempAuthor,
                Frequencies = tempList
            };

            analysis.Add(authorObject);
        }

        var options = new JsonSerializerOptions { WriteIndented = true };
        var jsonString = JsonSerializer.Serialize(analysis, options);

        return jsonString;
    }
}