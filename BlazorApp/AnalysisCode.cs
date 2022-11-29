namespace BlazorApp;
using GitInsight.Entities.DTOS;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Linq;

public sealed class AnalysisCode
{
    private static AnalysisCode instance = null;
    private static readonly object padlock = new object();
    public AuthorDTO[] authorAnalysis { get; set; }
    public AuthorObject[] authorObjects { get; set; }
    public FrequencyDTO[] frequencyAnalysis { get; set; }
    public ForkDTO[] forkAnalysis { get; set; }
    public FileDTO[] fileAnalysis { get; set; }

    private AnalysisCode() 
    {
        authorAnalysis = new AuthorDTO[] {};
        authorObjects = new AuthorObject[] {};
        frequencyAnalysis = new FrequencyDTO[] {};
        forkAnalysis = new ForkDTO[] {}; 
        fileAnalysis = new FileDTO[] {};
    }

    public static AnalysisCode Instance 
    {
        get
        {
            lock(padlock)
            {
                if(instance == null)
                {
                    instance = new AnalysisCode();
                }
                return instance;
            }
        }
    }

    public string? repository { get; set; }

    public void OnChange(string value, string name)
    {
        repository = value;
    }

    public AuthorObject[] convertToAuthorObjects() 
    {
        var authors = new AuthorObject[authorAnalysis.Length];
        for(int i = 0; i < authorAnalysis.Length; i++) 
        {
            AuthorObject author = new AuthorObject{author = authorAnalysis[i].author, frequency = authorAnalysis[i].frequencies.Sum(item => item.frequency)};
            authors[i] = author;
        }
        return authors;
    }

    public record AuthorObject 
    {
        public string? author {get; set;}
        public int frequency {get; set;}
    }
}