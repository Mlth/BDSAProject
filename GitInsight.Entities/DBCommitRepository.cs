using GitInsight.Core;
using System.Collections.ObjectModel;
using GitInsight;

namespace GitInsight.Entities;

public class DBCommitRepository
{
    RepositoryContext _context;
    public DBCommitRepository(RepositoryContext _context){
        this._context = _context;
    }
    public (Response Response, int UserId) Create(DBCommitCreateDTO DTO)
    {
        DBCommitCreateDTO xd = new DBCommitCreateDTO(DTO.frequency, DTO.repoID, DTO.author, DTO.date);
        /*DBCommitCreateDTO DBC = new()
        {
            frequency = DTO.frequency,
            repoID = DTO.repoID,
            author = DTO.author,
            date = DTO.date,
        };*/
        return (Response.Created, DTO.repoID);
    }

    public Response Delete()
    {
        throw new NotImplementedException();
    }

    public DBCommit Read()
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<DBCommit> ReadAll()
    {
        var list = new List<DBCommit>();
        foreach (var commit in _context.Commits)
        {
            list.Add(new DBCommit(commit.frequency, commit.repoID, commit.author, commit.date));
        }
        return new ReadOnlyCollection<DBCommit>(list);
    }

    public Response Update(DBCommitUpdateDTO DBC)
    {

        var entry = _context.CommitData.Where(a => a.author == DBC.author).First();
        if (entry == null) return Response.NotFound;

        //entry.frequency = DBC.frequency;
        //entry.repoID = DBC.repoID;
        throw new NotImplementedException();
        //return Response.Updated;
    }
}