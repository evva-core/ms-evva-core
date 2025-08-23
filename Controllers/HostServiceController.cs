using Microsoft.AspNetCore.Mvc;
using ms_evva_core.Base;
using ms_evva_core.Models;
using ms_evva_core.Services.Interfaces;

namespace ms_evva_core.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class HostServiceController : GenericController<HostService>
    {
        private readonly IHostServiceControllerService _hostService;

        public HostServiceController(IHostServiceControllerService hostService) 
            : base(hostService)
        {
            _hostService = hostService;
        }
    }
}
