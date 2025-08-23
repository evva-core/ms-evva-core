using Microsoft.AspNetCore.Mvc;
using ms_evva_core.Base;
using ms_evva_core.Models;
using ms_evva_core.Services.Interfaces;

namespace ms_evva_core.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class HostMetricController : GenericController<HostMetric>
    {
        private readonly IHostMetricControllerService _hostMetricService;

        public HostMetricController(IHostMetricControllerService hostMetricService) 
            : base(hostMetricService)
        {
            _hostMetricService = hostMetricService;
        }
    }
}
