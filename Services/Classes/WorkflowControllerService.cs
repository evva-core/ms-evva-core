using ms_evva_core.Base;
using ms_evva_core.Models;
using ms_evva_core.Repos.Interfaces;
using ms_evva_core.Services.Interfaces;

namespace ms_evva_core.Services.Classes;

public class WorkflowControllerService : GenericControllerService<Workflow>, IWorkflowControllerService
{
    private readonly IWorkflowRepository _workflowRepository;

    public WorkflowControllerService(IWorkflowRepository repository) : base(repository)
    {
        _workflowRepository = repository;
    }

    public async Task<ApiResponse<IEnumerable<Workflow>>> GetAvailableWorkflowsAsync()
    {
        try
        {
            var workflows = await _workflowRepository.GetAvailableWorkflowsAsync();
            return new ApiResponse<IEnumerable<Workflow>>
            {
                Success = true,
                Data = workflows
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<Workflow>>
            {
                Success = false,
                Message = ex.Message
            };
        }
    }
} 