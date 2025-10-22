using ms_evva_core.Repos.Interfaces;
using ms_evva_core.Models;

namespace ms_evva_core.Repos.Interfaces;

public interface IWorkflowRepository : IRepository<Models.Workflow>
{
    Task<IEnumerable<Workflow>> GetAvailableWorkflowsAsync();
} 