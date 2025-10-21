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

    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok(new { message = "Workflow controller is working", timestamp = DateTime.UtcNow });
    }
} 