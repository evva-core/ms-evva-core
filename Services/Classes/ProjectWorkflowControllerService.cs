using ms_evva_core.Base;
using ms_evva_core.Models;
using ms_evva_core.Repos.Interfaces;
using ms_evva_core.Services.Interfaces;

namespace ms_evva_core.Services.Classes;

public class ProjectWorkflowControllerService : GenericControllerService<ProjectWorkflow>, IProjectWorkflowControllerService
{
    private readonly IProjectWorkflowRepository _projectWorkflowRepository;

    public ProjectWorkflowControllerService(IProjectWorkflowRepository repository) : base(repository)
    {
        _projectWorkflowRepository = repository;
    }

    public async Task<IEnumerable<ProjectWorkflow>> GetByProjectIdAsync(int projectId)
    {
        return await _projectWorkflowRepository.GetByProjectIdAsync(projectId);
    }
} 