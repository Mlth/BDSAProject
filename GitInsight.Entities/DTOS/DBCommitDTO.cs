namespace GitInsight.Entities.DTOS;

public record DBCommitCreateDTO(int frequency, string repoID, string author, DateTime date);

public record DBCommitUpdateDTO(int frequency, string repoID, string author, DateTime date);
