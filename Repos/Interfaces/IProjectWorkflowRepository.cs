using ms_evva_core.Repos.Interfaces;
using ms_evva_core.Models;

namespace ms_evva_core.Repos.Interfaces;

public interface IProjectWorkflowRepository : IRepository<Models.ProjectWorkflow>
{
    Task<IEnumerable<ProjectWorkflow>> GetByProjectIdAsync(int projectId);
} 