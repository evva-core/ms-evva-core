using ms_evva_core.Models;
using ms_evva_core.Repos.Interfaces;

namespace ms_evva_core.Repos.Interfaces
{
    public interface IRepositoryRepository : IRepository<Repository>
    {
        Task<IEnumerable<Repository>> GetByProjectIdAsync(int projectId);
    }
}
