using ms_evva_core.Base;
using ms_evva_core.Models;
using ms_evva_core.Repos.Interfaces;
using ms_evva_core.Services.Interfaces;
using ms_evva_core.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace ms_evva_core.Services.Classes;

public class ProjectWorkflowControllerService : GenericControllerService<ProjectWorkflow>, IProjectWorkflowControllerService
{
    private readonly IProjectWorkflowRepository _projectWorkflowRepository;
    private readonly IServiceProvider _serviceProvider;

    public ProjectWorkflowControllerService(IProjectWorkflowRepository repository, IServiceProvider serviceProvider) : base(repository)
    {
        _projectWorkflowRepository = repository;
        _serviceProvider = serviceProvider;
    }

    public async Task<IEnumerable<ProjectWorkflow>> GetByProjectIdAsync(int projectId)
    {
        return await _projectWorkflowRepository.GetByProjectIdAsync(projectId);
    }

    public async Task SaveProjectWorkflowsAsync(int projectId, IEnumerable<ProjectWorkflowController.ProjectWorkflowDto> workflows)
    {
        var existingWorkflows = await _projectWorkflowRepository.GetByProjectIdAsync(projectId);
        var workflowRepository = _serviceProvider.GetRequiredService<IWorkflowRepository>();
        var projectRepository = _serviceProvider.GetRequiredService<IProjectRepository>();
        var project = await projectRepository.GetByIdAsync(projectId);
        
        foreach (var workflow in workflows)
        {
            if (workflow.Id > 0 && workflow.Id < int.MaxValue)
            {
                // Update existing workflow
                var existing = existingWorkflows.FirstOrDefault(e => e.Id == (int)workflow.Id);
                if (existing != null)
                {
                    // Check if data changed (user modified template)
                    var templateWorkflow = await workflowRepository.GetByIdAsync(existing.WorkflowId);
                    bool dataChanged = templateWorkflow?.IsJsonRequired == true 
                        ? existing.Workflow?.JsonData != workflow.Workflow?.JsonData
                        : existing.Workflow?.Parameters != workflow.Workflow?.JsonData;
                    
                    if (dataChanged && !string.IsNullOrEmpty(workflow.Workflow?.JsonData))
                    {
                        // Create new workflow with user changes
                        var newWorkflow = new Workflow
                        {
                            Name = templateWorkflow?.Name?.Replace("[TEMPLATE]", $"[{project?.Name}]") ?? "Custom Workflow",
                            Command = templateWorkflow?.Command ?? "",
                            Description = templateWorkflow?.Description,
                            SupportedOs = templateWorkflow?.SupportedOs ?? Models.Enums.SupportedOs.Linux,
                            Parameters = templateWorkflow?.IsJsonRequired == false ? workflow.Workflow.JsonData : templateWorkflow?.Parameters,
                            IsJsonRequired = templateWorkflow?.IsJsonRequired ?? false,
                            JsonData = templateWorkflow?.IsJsonRequired == true ? workflow.Workflow.JsonData : null
                        };
                        
                        var newWorkflowId = await workflowRepository.AddAsync(newWorkflow);
                        existing.WorkflowId = newWorkflowId;
                    }
                    
                    // Update project workflow properties
                    existing.ExecutionOrder = workflow.ExecutionOrder;
                    existing.StageName = workflow.StageName;
                    await _projectWorkflowRepository.UpdateAsync(existing);
                }
            }
            else
            {
                // Create new project workflow
                var templateWorkflow = await workflowRepository.GetByIdAsync(workflow.WorkflowId);
                if (templateWorkflow != null)
                {
                    int workflowIdToUse = workflow.WorkflowId;
                    
                    // If user modified data, create new workflow
                    bool userModified = templateWorkflow.IsJsonRequired 
                        ? (!string.IsNullOrEmpty(workflow.Workflow?.JsonData) && workflow.Workflow.JsonData != templateWorkflow.JsonData)
                        : (!string.IsNullOrEmpty(workflow.Workflow?.JsonData) && workflow.Workflow.JsonData != templateWorkflow.Parameters);
                        
                    if (userModified)
                    {
                        var newWorkflow = new Workflow
                        {
                            Name = templateWorkflow.Name.Replace("[TEMPLATE]", $"[{project?.Name}]"),
                            Command = templateWorkflow.Command,
                            Description = templateWorkflow.Description,
                            SupportedOs = templateWorkflow.SupportedOs,
                            Parameters = templateWorkflow.IsJsonRequired ? templateWorkflow.Parameters : workflow.Workflow.JsonData,
                            IsJsonRequired = templateWorkflow.IsJsonRequired,
                            JsonData = templateWorkflow.IsJsonRequired ? workflow.Workflow.JsonData : null
                        };
                        
                        workflowIdToUse = await workflowRepository.AddAsync(newWorkflow);
                    }
                    
                    // Create project workflow link
                    var newProjectWorkflow = new ProjectWorkflow
                    {
                        ProjectId = projectId,
                        WorkflowId = workflowIdToUse,
                        ExecutionOrder = workflow.ExecutionOrder,
                        StageName = workflow.StageName
                    };
                    
                    await _projectWorkflowRepository.AddAsync(newProjectWorkflow);
                }
            }
        }
        
        // Remove deleted workflows
        var currentIds = workflows.Where(w => w.Id > 0 && w.Id < int.MaxValue).Select(w => (int)w.Id).ToList();
        var toRemove = existingWorkflows.Where(e => !currentIds.Contains(e.Id));
        foreach (var remove in toRemove)
        {
            await _projectWorkflowRepository.DeleteAsync(remove.Id);
        }
    }
} 