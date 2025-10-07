using ms_evva_core.Models;

namespace ms_evva_core.Services.Interfaces;

public interface IProjectWorkflowControllerService : IControllerService<ProjectWorkflow>
{
    Task<IEnumerable<ProjectWorkflow>> GetByProjectIdAsync(int projectId);
} 