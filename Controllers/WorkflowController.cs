using Microsoft.AspNetCore.Mvc;
using ms_evva_core.Base;
using ms_evva_core.Models;
using ms_evva_core.Services.Interfaces;

namespace ms_evva_core.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class WorkflowController : GenericController<Workflow>
{
    private readonly IWorkflowControllerService _workflowService;

    public WorkflowController(IWorkflowControllerService workflowService) 
        : base(workflowService)
    {
        _workflowService = workflowService;
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableWorkflows()
    {
        var response = await _workflowService.GetAvailableWorkflowsAsync();
        return response.Success ? Ok(response) : BadRequest(response);
    }
} 