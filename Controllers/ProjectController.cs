using Microsoft.AspNetCore.Mvc;
using ms_evva_core.Base;
using ms_evva_core.Models;
using ms_evva_core.Services.Interfaces;
using System.Threading.Tasks;

namespace ms_evva_core.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProjectController : GenericController<Project>
{
    private readonly IProjectControllerService _projectService;

    public ProjectController(IProjectControllerService projectService) 
        : base(projectService)
    {
        _projectService = projectService;
    }

    [HttpGet("GetProjectsWithDetails")]
    [Route("details")]
    public async Task<IActionResult> GetAllWithDetails()
    {
        return await _projectService.GetAllProjectsWithDetailsAsync();
    }

    [HttpGet("getProjectWithDetailsWithId")]
    [Route("details/{id}")]
    public async Task<IActionResult> GetProjectWithDetails(int id)
    {
        return await _projectService.GetProjectWithDetailsAsync(id);
    }
}