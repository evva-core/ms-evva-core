using Microsoft.AspNetCore.SignalR;
using ms_evva_core.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace ms_evva_core.Hubs
{
    public class HostHub : Hub
    {
        private readonly IHubContext<ProjectHub> _projectHub;
        private readonly IActiveHostService _activeHostService;

        public HostHub(IHubContext<ProjectHub> projectHub, IActiveHostService activeHostService)
        {
            _projectHub = projectHub;
            _activeHostService = activeHostService;
        }
        public async Task JoinHostGroup(string uniqueId)
        {
            Console.WriteLine($"[HostHub] JoinHostGroup invoked for uniqueId: {uniqueId} by ConnectionId: {Context.ConnectionId}");
            await Groups.AddToGroupAsync(Context.ConnectionId, uniqueId);
            Console.WriteLine($"Client {Context.ConnectionId} joined group {uniqueId}.");
        }

        public async Task LeaveHostGroup(string uniqueId)
        {
            Console.WriteLine($"[HostHub] LeaveHostGroup invoked for uniqueId: {uniqueId} by ConnectionId: {Context.ConnectionId}");
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, uniqueId);
            Console.WriteLine($"Client {Context.ConnectionId} left group {uniqueId}.");
        }

        public async Task SendHostData(string uniqueId, object data)
        {
            Console.WriteLine($"[HostHub] SendHostData invoked for uniqueId: {uniqueId}");
            
            // Update active host list
            _activeHostService.UpdateHostActivity(uniqueId, data);
            
            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(data, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                
                // Repassa dados para frontend
                if (data is System.Text.Json.JsonElement element && element.TryGetProperty("type", out var typeProperty))
                {
                    var type = typeProperty.GetString();
                    
                    // Repository events
                    if (type?.StartsWith("repository_") == true && element.TryGetProperty("repositoryId", out var repoIdProperty))
                    {
                        var repositoryId = repoIdProperty.GetInt32();
                        var projectId = 1; // placeholder
                        
                        if (type == "repository_clone_completed")
                            await _projectHub.Clients.Group($"project-{projectId}").SendAsync("RepositoryCloneCompleted", new { repositoryId });
                        else if (type == "repository_clone_failed")
                            await _projectHub.Clients.Group($"project-{projectId}").SendAsync("RepositoryCloneFailed", new { repositoryId });
                    }
                    
                    // Deployment events
                    else if (type?.StartsWith("deployment_") == true && element.TryGetProperty("projectId", out var projIdProperty))
                    {
                        var projectId = projIdProperty.GetInt32();
                        
                        if (type == "deployment_progress")
                        {
                            var stage = element.GetProperty("stage").GetString();
                            var message = element.GetProperty("message").GetString();
                            var isError = element.TryGetProperty("isError", out var errorProp) && errorProp.GetBoolean();
                            await _projectHub.Clients.Group($"project-{projectId}").SendAsync("DeploymentProgress", new { projectId, stage, message, isError, timestamp = DateTime.UtcNow });
                        }
                        else if (type == "deployment_completed")
                        {
                            var success = element.GetProperty("success").GetBoolean();
                            var message = element.GetProperty("message").GetString();
                            await _projectHub.Clients.Group($"project-{projectId}").SendAsync("DeploymentCompleted", new { projectId, success, message, timestamp = DateTime.UtcNow });
                        }
                    }
                }
                
                await Clients.Group(uniqueId).SendAsync("ReceiveHostData", data);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing data from host {uniqueId}: {ex.Message}");
            }
        }

        public async Task CloneRepository(int repositoryId, string url, string branch, string targetPath)
        {
            Console.WriteLine($"[HostHub] CloneRepository invoked for repo {repositoryId}");
            // Este método será chamado pelos agents conectados
            await Clients.Caller.SendAsync("ExecuteClone", new { repositoryId, url, branch, targetPath });
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"[HostHub] OnDisconnectedAsync invoked for ConnectionId: {Context.ConnectionId}");
            // You might need to handle group cleanup if a client disconnects unexpectedly.
            // This could involve a mapping of ConnectionId to the groups it has joined.
            // For simplicity, we'll leave this for now.
            await base.OnDisconnectedAsync(exception);
        }
    }
}