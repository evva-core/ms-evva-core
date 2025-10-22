using ms_evva_core.Models;

namespace ms_evva_core.Services.Interfaces;

public interface IWorkflowControllerService : IControllerService<Workflow>
{
    Task<ApiResponse<IEnumerable<Workflow>>> GetAvailableWorkflowsAsync();
} 