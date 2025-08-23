using ms_evva_core.Base;
using ms_evva_core.Models;
using ms_evva_core.Repos.Interfaces;
using ms_evva_core.Services.Interfaces;

namespace ms_evva_core.Services.Classes
{
    public class HostMetricControllerService : GenericControllerService<HostMetric>, IHostMetricControllerService
    {
        public HostMetricControllerService(IHostMetricRepository repository) : base(repository)
        {
        }
    }
}
