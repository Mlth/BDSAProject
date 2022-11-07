namespace GitInsight.Entities;
    public class DBRepoRepository
    {
        private RepositoryContext _context;

        public DBRepoRepository(RepositoryContext context)
        {
            _context = context;
        }

        public DBRepositoryDTO Find(String repoID){
            var repo = (from c in _context.Repository
                 where c.id == repoID
                 select c).FirstOrDefault();
            if (repo is null)
            {
                return null;
            }
            else{
                return new DBRepositoryDTO{
                    Id = repoID.Id,
                    state = repo.state;
                }
            }
        }
    }