using Microsoft.AspNetCore.SignalR;
using ms_evva_core.Models.Dtos;
using ms_evva_core.Services.Interfaces;

namespace ms_evva_core.Hubs;

public class ProjectHub : Hub
{
    private readonly IHubContext<ProjectHub> _hubContext;
    private readonly IProjectControllerService _projectService;

    public ProjectHub(IHubContext<ProjectHub> hubContext, IProjectControllerService projectService)
    {
        _hubContext = hubContext;
        _projectService = projectService;
    }

    public async Task JoinProjectGroup(int projectId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"project-{projectId}");
    }

    public async Task LeaveProjectGroup(int projectId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"project-{projectId}");
    }

    public async Task StartDeploy(int projectId)
    {
        try
        {
            var deployRequest = await _projectService.CreateDeploymentRequest(projectId);
            
            // Notify project group that deployment started
            await _hubContext.Clients.Group($"project-{projectId}")
                .SendAsync("DeploymentStarted", new { projectId, deploymentId = deployRequest.Id });

            // Send deployment request to agent via HostHub using UUID
            var hostHub = Context.GetHttpContext()?.RequestServices.GetService<IHubContext<HostHub>>();
            if (hostHub != null)
            {
                await hostHub.Clients.Group(deployRequest.HostUniqueId)
                    .SendAsync("ExecuteDeployment", deployRequest);
            }
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("DeploymentError", new { projectId, error = ex.Message });
        }
    }

    public async Task NotifyDeploymentProgress(int projectId, string stage, string message, bool isError = false)
    {
        System.Console.WriteLine($"[ProjectHub] NotifyDeploymentProgress: projectId={projectId}, stage={stage}, message={message}, isError={isError}");
        await _hubContext.Clients.Group($"project-{projectId}")
            .SendAsync("DeploymentProgress", new { projectId, stage, message, isError, timestamp = DateTime.UtcNow });
    }

    public async Task NotifyDeploymentCompleted(int projectId, bool success, string? message = null)
    {
        await _hubContext.Clients.Group($"project-{projectId}")
            .SendAsync("DeploymentCompleted", new { projectId, success, message, timestamp = DateTime.UtcNow });
    }
}