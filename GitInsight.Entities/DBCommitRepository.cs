using GitInsight.Entities.DTOS;
using System.Collections.ObjectModel;
using GitInsight;

namespace GitInsight.Entities;

public class DBCommitRepository
{
    RepositoryContext _context;
    public DBCommitRepository(RepositoryContext _context){
        this._context = _context;
    }
    public Response Create(DBCommit entity)
    {
    
        _context.CommitData.Add(entity);
        _context.SaveChanges();
        return Response.Created;
    }

    

    public Response CreateOrUpdate(DBCommit commit){
        if (Read(commit) is null){
            return Create(commit);
        }
        return Update(commit);
    }

    public Response Delete()
    {
        throw new NotImplementedException();
    }

    public DBCommit Read(DBCommit commit)
    {
        var entity = (from c in _context.CommitData
                 where c.author == commit.author && c.date == commit.date && c.repo.name == commit.repo.name 
                 select c).FirstOrDefault();
        if (entity is null)
        {
            return null;
        }
        return entity;
    }

    public IReadOnlyCollection<DBCommit> ReadAll()
    {
        var list = new List<DBCommit>();
        foreach (var commit in _context.CommitData)
        {
            list.Add(commit);
        }
        return new ReadOnlyCollection<DBCommit>(list);
    }

    public Response Update(DBCommit entity)
    {   var commit = Read(entity);
        if (commit is null){
            return Response.NotFound;
        }
        commit = entity;
        _context.SaveChanges();
        return Response.Updated;
    }
}