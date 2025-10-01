using Microsoft.AspNetCore.Mvc;
using ms_evva_core.Base;
using ms_evva_core.Models;
using ms_evva_core.Models.Enums;
using ms_evva_core.Repos.Interfaces;
using ms_evva_core.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace ms_evva_core.Services.Classes;

public class ProjectControllerService : GenericControllerService<Project>, IProjectControllerService
{
    private readonly IProjectRepository _projectRepository;
    private readonly IRepositoryRepository _repositoryRepository;
    public ProjectControllerService(IProjectRepository repository, IRepositoryRepository repositoryRepository) : base(repository)
    {
        _projectRepository = repository;
        _repositoryRepository = repositoryRepository;
    }

    public async Task<IActionResult> AddAsync(Models.Dtos.ProjetoRepositoryDto entity)
    {
        try
        {
            entity.Status = ProjectStatus.Active;
            await _projectRepository.AddAsync(entity);
            foreach (var repo in entity.Repositories)
            {
                repo.ProjectId = entity.Id;
                await _repositoryRepository.AddAsync(repo);
            }
            return Ok(entity);
        }
        catch (Exception ex)
        {
          return BadRequest(ex.Message);
        }
    
    }

    public async Task<IActionResult> GetAllProjectsWithDetailsAsync()
    {
        try
        {
            var projects = await _projectRepository.GetAllProjectsWithDetailsAsync();
            return new OkObjectResult(projects);
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex.Message);
        }
    }

    public async Task<IActionResult> GetProjectWithDetailsAsync(int id)
    {
        try
        {
            var project = await _projectRepository.GetProjectWithDetailsAsync(id);
            if (project == null)
            {
                return new NotFoundObjectResult($"Project with ID {id} not found");
            }
            return new OkObjectResult(new { success = true, data = project });
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(new { success = false, message = ex.Message });
        }
    }
}