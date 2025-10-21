using Microsoft.AspNetCore.Mvc;
using ms_evva_core.Models;
using ms_evva_core.Models.Dtos;
using System.Threading.Tasks;

namespace ms_evva_core.Services.Interfaces;

public interface IProjectControllerService : IControllerService<Project>
{
    Task<IActionResult> GetAllProjectsWithDetailsAsync();
    Task<IActionResult> GetProjectWithDetailsAsync(int id);
    Task<DeploymentRequestDto> CreateDeploymentRequest(int projectId);
}