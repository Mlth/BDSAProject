namespace GitInsight.Entities.DTOS;

public record DBRepositoryDTO{
    public string name {get; set;}

    public string state {get; set;}

    public ICollection<DBCommit> commits {get; set;}

}