using Microsoft.AspNetCore.Mvc;
using ms_evva_core.Base;
using ms_evva_core.Models;
using ms_evva_core.Services.Interfaces;

namespace ms_evva_core.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProjectWorkflowController : GenericController<ProjectWorkflow>
{
    private readonly IProjectWorkflowControllerService _projectWorkflowService;

    public ProjectWorkflowController(IProjectWorkflowControllerService projectWorkflowService) 
        : base(projectWorkflowService)
    {
        _projectWorkflowService = projectWorkflowService;
    }

    [HttpGet("project/{projectId}")]
    public async Task<IActionResult> GetByProjectId(int projectId)
    {
        var workflows = await _projectWorkflowService.GetByProjectIdAsync(projectId);
        return Ok(new ApiResponse<IEnumerable<ProjectWorkflow>>
        {
            Data = workflows,
            Success = true
        });
    }

    [HttpPost("project/{projectId}/bulk")]
    public async Task<IActionResult> SaveProjectWorkflows(int projectId, [FromBody] SaveProjectWorkflowsRequest request)
    {
        try
        {
            await _projectWorkflowService.SaveProjectWorkflowsAsync(projectId, request.Workflows);
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Workflows saved successfully"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    public class SaveProjectWorkflowsRequest
    {
        public IEnumerable<ProjectWorkflowDto> Workflows { get; set; } = new List<ProjectWorkflowDto>();
    }

    public class ProjectWorkflowDto
    {
        public long Id { get; set; }
        public int ProjectId { get; set; }
        public int WorkflowId { get; set; }
        public int ExecutionOrder { get; set; }
        public string? StageName { get; set; }
        public WorkflowDto Workflow { get; set; } = new();
    }

    public class WorkflowDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Command { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string SupportedOs { get; set; } = string.Empty;
        public string? Parameters { get; set; }
        public bool IsJsonRequired { get; set; }
        public string? JsonData { get; set; }
    }
} 