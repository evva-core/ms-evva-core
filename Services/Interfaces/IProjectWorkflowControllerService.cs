using ms_evva_core.Models;
using ms_evva_core.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace ms_evva_core.Services.Interfaces;

public interface IProjectWorkflowControllerService : IControllerService<ProjectWorkflow>
{
    Task<IActionResult> GetByProjectIdAsync(int projectId);
    Task SaveProjectWorkflowsAsync(int projectId, IEnumerable<ProjectWorkflowController.ProjectWorkflowDto> workflows);
} 