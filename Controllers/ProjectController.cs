using Microsoft.AspNetCore.Mvc;
using ms_evva_core.Base;
using ms_evva_core.Models;
using ms_evva_core.Services.Interfaces;

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
}  