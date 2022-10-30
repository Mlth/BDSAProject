namespace GitInsight;
using LibGit2Sharp;

public class AuthorCommand : Command {

    List<AuthorDTO> authors {get;} = new List<AuthorDTO>();

    public void execute(Repository repo){
        string specifier = "d";
        var commitlog = repo.Commits;
        var authorMap = new Dictionary<string, Dictionary<string, int>>();
        foreach (Commit c in commitlog){
            if (!authorMap.ContainsKey(c.Author.Name)){
                authorMap.Add(c.Author.Name, new Dictionary<string, int>());
            }
            var dateMap = authorMap[c.Author.Name];
            if (dateMap.ContainsKey(c.Author.When.ToString(specifier))){
                dateMap[c.Author.When.ToString(specifier)]++;
            }
            else{
                dateMap[c.Author.When.ToString(specifier)] = 1;
            }
        }

        foreach (string author in authorMap.Keys){
            AuthorDTO dto = new AuthorDTO{
                Author = author,
                frequencies = new List<FrequencyDTO>()
            };
            foreach (string date in authorMap[author].Keys){

                dto.frequencies.Add(new FrequencyDTO{
                dateTime = date,
                frequency = authorMap[author][date]});
            }
            authors.Add(dto);
        }
    }

    public IVisualizer getVisualizer(){
        return new AuthorVisualizer(authors);
    }

}