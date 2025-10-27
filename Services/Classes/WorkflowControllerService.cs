using Microsoft.AspNetCore.Mvc;
using ms_evva_core.Base;
using ms_evva_core.Models;
using ms_evva_core.Repos.Interfaces;
using ms_evva_core.Services.Interfaces;
using ms_evva_core.Utils;

namespace ms_evva_core.Services.Classes;

public class WorkflowControllerService : GenericControllerService<Workflow>, IWorkflowControllerService
{
    private readonly IWorkflowRepository _workflowRepository;
    private readonly ErrorHandler errorHandler = new();

    public WorkflowControllerService(IWorkflowRepository repository) : base(repository)
    {
        _workflowRepository = repository;
    }

    public async Task<IActionResult> GetAvailableWorkflowsAsync()
    {
        try
        {
            var workflows = await _workflowRepository.GetAvailableWorkflowsAsync();
            return Ok(workflows);
        }
        catch (Exception ex)
        {
            return ErrorHandler.InvokeError(ex);
        }
    }
} 