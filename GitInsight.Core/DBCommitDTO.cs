namespace GitInsight.Core;

public record DBCommit(){
    public int frequency {get; set;}
    public string repoID {get; set;}

    public string author {get; set;}

    public DateTime date {get; set;}
};

public record DBCommitCreateDTO(int frequency, string repoID, string author, DateTime date);

public record DBCommitUpdateDTO(int frequency, string repoID, string author, DateTime date);
