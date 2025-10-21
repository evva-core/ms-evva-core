using ms_evva_core.Models;
using ms_evva_core.Controllers;

namespace ms_evva_core.Services.Interfaces;

public interface IProjectWorkflowControllerService : IControllerService<ProjectWorkflow>
{
    Task<IEnumerable<ProjectWorkflow>> GetByProjectIdAsync(int projectId);
    Task SaveProjectWorkflowsAsync(int projectId, IEnumerable<ProjectWorkflowController.ProjectWorkflowDto> workflows);
} 