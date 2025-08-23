using ms_evva_core.Base;
using ms_evva_core.Models;
using ms_evva_core.Repos.Interfaces;

namespace ms_evva_core.Repos.Classes
{
    public class HostServiceRepository : GenericRepository<HostService>, IHostServiceRepository
    {
        public HostServiceRepository() : base("host_services")
        {
        }
    }
}
