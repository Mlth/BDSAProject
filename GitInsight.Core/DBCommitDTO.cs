namespace GitInsight.Entities;
using GitInsight.Core;

public record DBCommit(int frequency, int repoID, string author, DateTime date);

public record DBCommitCreateDTO(int frequency, int repoID, string author, DateTime date);

public record DBCommitUpdateDTO(int frequency, int repoID, string author, DateTime date);
