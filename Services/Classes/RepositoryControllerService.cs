using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ms_evva_core.Base;
using ms_evva_core.Hubs;
using ms_evva_core.Models;
using ms_evva_core.Repos.Interfaces;
using ms_evva_core.Services.Interfaces;

namespace ms_evva_core.Services.Classes
{
    public class RepositoryControllerService : GenericControllerService<Repository>, IRepositoryControllerService
    {
        private readonly IHubContext<ProjectHub> _projectHub;
        private readonly IHubContext<HostHub> _hostHub;
        private readonly IRepositoryRepository _repo;
        private readonly IHostRepository _hostRepo;
        public RepositoryControllerService(IRepositoryRepository repository,
        IHubContext<ProjectHub> projectHub,
        IHubContext<HostHub> hostHub,
        IHostRepository hostRepo
        ) :
        base(repository)
        {
            _projectHub = projectHub;
            _repo = repository;
            _hostHub = hostHub;
            _hostRepo = hostRepo;
        }

         public async Task<IActionResult> CloneRepository(int id)
        {
            var repo = await _repo.GetByIdAsync(id);
            if (repo == null) return NotFound();

            var host = await _hostRepo.GetByIdAsync(repo.HostId);
            if (host == null) return BadRequest("Host not found");

            // Notifica frontend que clone iniciou
            await _projectHub.Clients.Group($"project-{repo.ProjectId}")
                .SendAsync("RepositoryCloneStarted", new { repositoryId = id, status = "cloning" });

            // Envia comando para agent usando uniqueId do host
            await _hostHub.Clients.Group(host.UniqueId)
                .SendAsync("ExecuteClone", new { repositoryId = id, url = repo.RepositoryUrl, branch = repo.Branch, targetPath = repo.TargetPath });

            return Ok(new ApiResponse<object> { Success = true, Message = "Clone started" });
        }
    }
}
