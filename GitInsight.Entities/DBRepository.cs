using System.ComponentModel.DataAnnotations;

namespace GitInsight.Entities;

public record DBRepository
{
    public int Id {get; set;}
    public string name { get; set; }

    public string state { get; set; }

    public ICollection<DBCommit> commits {get; set;}
}