namespace GitInsight.Entities;
public record DBCommit(){

    public string author {get; set;}

    public DateTime date {get; set;}

    public DBRepository repo {get; set;}
};