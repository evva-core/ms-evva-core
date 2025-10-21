using Microsoft.AspNetCore.Mvc;
using ms_evva_core.Base;
using ms_evva_core.Models;
using ms_evva_core.Models.Dtos;
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
    private readonly IHostRepository _hostRepository;
    public ProjectControllerService(IProjectRepository repository, IRepositoryRepository repositoryRepository, IHostRepository hostRepository) : base(repository)
    {
        _projectRepository = repository;
        _repositoryRepository = repositoryRepository;
        _hostRepository = hostRepository;
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

    public async Task<DeploymentRequestDto> CreateDeploymentRequest(int projectId)
    {
        var project = await _projectRepository.GetProjectWithDetailsAsync(projectId);
        if (project == null)
            throw new ArgumentException($"Project with ID {projectId} not found");

        var repositories = await _repositoryRepository.GetByProjectIdAsync(projectId);
        var workflows = await _projectRepository.GetProjectWorkflowsAsync(projectId);

        // Get the first repository's host for deployment (could be enhanced to support multiple hosts)
        var hostId = repositories.FirstOrDefault()?.HostId ?? throw new InvalidOperationException("No repositories found for project");
        var host = await _hostRepository.GetByIdAsync(hostId);
        if (host == null)
            throw new InvalidOperationException($"Host with ID {hostId} not found");

        return new DeploymentRequestDto
        {
            Id = new Random().Next(1000, 9999), // Generate temporary ID
            ProjectId = projectId,
            HostId = hostId,
            HostUniqueId = host.UniqueId!,
            ProjectName = project.Name,
            Repositories = repositories.Select(r => new RepositoryDto
            {
                Id = r.Id,
                Name = r.Name,
                RepositoryUrl = r.RepositoryUrl,
                Branch = r.Branch,
                TargetPath = r.TargetPath,
                IsDockerEnabled = r.IsDockerEnabled ?? false,
                DockerConfigId = r.DockerConfigId
            }).ToList(),
            WorkflowSteps = workflows.Select(w => {
                Console.WriteLine($"[CreateDeploymentRequest] Workflow {w.Id}: JsonData = {w.Workflow?.JsonData ?? "NULL"}");
                return new WorkflowStepDto
                {
                    Id = w.Id,
                    ExecutionOrder = w.ExecutionOrder,
                    StageName = w.StageName ?? "Build",
                    WorkflowName = w.Workflow.Name,
                    Command = w.Workflow.Command,
                    Description = w.Workflow.Description,
                    SupportedOs = w.Workflow.SupportedOs.ToString(),
                    Parameters = w.Workflow.Parameters,
                    IsJsonRequired = w.Workflow.IsJsonRequired,
                    JsonData = w.Workflow.JsonData
                };
            }).OrderBy(w => w.ExecutionOrder).ToList(),
            CreatedAt = DateTime.UtcNow
        };
    }
}