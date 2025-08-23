using ms_evva_core.Base;
using ms_evva_core.Models;
using ms_evva_core.Repos.Interfaces;
using ms_evva_core.Services.Interfaces;

namespace ms_evva_core.Services.Classes
{
    public class RepositoryControllerService : GenericControllerService<Repository>, IRepositoryControllerService
    {
        public RepositoryControllerService(IRepositoryRepository repository) : base(repository)
        {
        }
    }
}
