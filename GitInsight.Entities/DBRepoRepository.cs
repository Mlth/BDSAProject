namespace GitInsight.Entities;
using GitInsight.Entities.DTOS;
using Microsoft.EntityFrameworkCore;
using GitInsight.Core;
using System.Linq;
    public class DBRepoRepository
    {
        private RepositoryContext _context;

        public DBRepoRepository(RepositoryContext context)
        {
            _context = context;
        }

        public Response Create(DBRepositoryDTO dto){
            var entity = (from c in _context.RepoData
                        where c.name == dto.name
                        select c).FirstOrDefault();

            if (entity is null)
            {
                entity = new DBRepository{name = dto.name, state = dto.state, commits = dto.commits};

                _context.RepoData.Add(entity);
                _context.SaveChanges();

                return Response.Created;
            }
                return Response.Conflict;
            
        }

        public (Response reponse, DBRepositoryDTO dto) Update(DBRepositoryDTO dto) {
            var entity = Read(dto);
            

            if (entity is null)
            {
                return (Response.NotFound, null);
            }
            else
            { 
                entity.name = dto.name;
                entity.state = dto.state;
                entity.commits = dto.commits;
                _context.RepoData.Update(entity);
                _context.SaveChanges();
                return (Response.Updated, new DBRepositoryDTO {name = entity.name, state = entity.state, commits = entity.commits});
            }
        }

        public DBRepository Read(DBRepositoryDTO dto){
            var repo = (from c in _context.RepoData
                 where c.name == dto.name
                 select c).FirstOrDefault();
            if (repo is null)
            {
                return null;
            }
            else{
                return repo;
            };
        }

        public ICollection<DBCommit> ReadAllCommits(DBRepositoryDTO dto){
            // var repo = (from c in _context.RepoData
            //      where c.name == dto.name
            //      select c).FirstOrDefault();

            var repo = _context.RepoData.Where(x => x.name == dto.name).Include(x => x.commits).FirstOrDefault();

            if (repo is null)
            {
                return null;
            }
            else{
                return repo.commits;
            };
        }
    }