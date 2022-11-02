namespace GitInsight;
using LibGit2Sharp;
using GitInsight.Core;

public class FrequencyCommand : Command {

    List<FrequencyDTO> frequencies = new List<FrequencyDTO>();

    public void execute(Repository repo){
        string specifier = "d";
        var commitlog = repo.Commits;
        var dateMap = new Dictionary<string, int>();
        foreach (Commit c in commitlog){
                if (dateMap.ContainsKey(c.Author.When.ToString(specifier))){
                    dateMap[c.Author.When.ToString(specifier)]++;
                }
                else{
                    dateMap[c.Author.When.ToString(specifier)] = 1;
                }
        }
        foreach (string date in dateMap.Keys){
            frequencies.Add(new FrequencyDTO{
                dateTime = date,
                frequency = dateMap[date]});
        }
    }

    public IVisualizer getVisualizer(){
        return new FrequencyVisualizer(frequencies);
    }
}