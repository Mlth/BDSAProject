namespace GitInsight.Entities.Tests;
using GitInsight.Entities;
using GitInsight.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Xunit;

public sealed class RepositoryCommitTests : IDisposable
{
    private readonly RepositoryContext _context;
    private readonly DBCommit _repository;

    public RepositoryCommitTests() {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<RepositoryContext>();
        builder.UseSqlite(connection);
        var context = new RepositoryContext(builder.Options);
        context.Database.EnsureCreated();
        
        context.SaveChanges();

        _context = context;
        //_repository = new DBCommit(); //_context?
    }

    public void Dispose() {
        _context.Dispose();
    }
}