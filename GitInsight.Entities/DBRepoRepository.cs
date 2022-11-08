namespace GitInsight.Entities;
using GitInsight.Entities.DTOS;
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
                entity = new DBRepository{name = dto.name, state = dto.state};

                _context.RepoData.Add(entity);
                _context.SaveChanges();

                return Response.Created;
            }
            else
            {
                return Response.Conflict;
            }
        }

        public DBRepositoryDTO Read(DBRepositoryDTO dto){
            var repo = (from c in _context.RepoData
                 where c.name == dto.name
                 select c).FirstOrDefault();
            if (repo is null)
            {
                return null;
            }
            else{
                return new DBRepositoryDTO{
                    name = repo.name,
                    state = repo.state,
                };
            }
        }
    }