using ms_evva_core.Base;
using ms_evva_core.Models;
using ms_evva_core.Repos.Interfaces;
using ms_evva_core.Utils;

namespace ms_evva_core.Repos.Classes
{
    public class RepositoryRepository : GenericRepository<Repository>, IRepositoryRepository
    {
        public RepositoryRepository() : base("repositories")
        {
        }
    }
}