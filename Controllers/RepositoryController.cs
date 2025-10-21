using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ms_evva_core.Base;
using ms_evva_core.Models;
using ms_evva_core.Services.Interfaces;
using ms_evva_core.Hubs;

namespace ms_evva_core.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RepositoryController : GenericController<Repository>
    {
        private readonly IRepositoryControllerService _repositoryService;
        private readonly IHubContext<ProjectHub> _projectHub;
        private readonly IHubContext<HostHub> _hostHub;

        public RepositoryController(
            IRepositoryControllerService repositoryService,
            IHubContext<ProjectHub> projectHub,
            IHubContext<HostHub> hostHub) 
            : base(repositoryService)
        {
            _repositoryService = repositoryService;
            _projectHub = projectHub;
            _hostHub = hostHub;
        }

        [HttpPost("{id}/clone")]
        public async Task<IActionResult> CloneRepository(int id)
        {
            return await _repositoryService.CloneRepository(id);
        }
    }
}
