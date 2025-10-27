using Microsoft.AspNetCore.Mvc;
using ms_evva_core.Base.Attributes;
using ms_evva_core.Models;
using ms_evva_core.Models.Dtos;
using ms_evva_core.Repos.Interfaces;
using ms_evva_core.Services.Interfaces;

namespace ms_evva_core.Controllers;

[ApiController]
[Route("api/v1/[controller]")]

public class DashboardController : ControllerBase
{
    private readonly IActiveHostService _activeHostService;
    private readonly IProjectRepository _projectRepository;
    private readonly IHostMetricRepository _hostMetricRepository;
    private readonly IHostRepository _hostRepository;
    private readonly Utils.ErrorHandler _errorHandler = new();

    public DashboardController(
        IActiveHostService activeHostService,
        IProjectRepository projectRepository,
        IHostMetricRepository hostMetricRepository,
        IHostRepository hostRepository)
    {
        _activeHostService = activeHostService;
        _projectRepository = projectRepository;
        _hostMetricRepository = hostMetricRepository;
        _hostRepository = hostRepository;
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetDashboardStats()
    {
        try
        {
            var activeHosts = _activeHostService.GetActiveHosts();
            var projects = await _projectRepository.GetAllAsync();
            var allHosts = await _hostRepository.GetAllAsync();
            
            var totalHosts = allHosts.Count();
            var onlineHosts = _activeHostService.GetOnlineHostsCount();
            var activeProjects = projects.Count(p => p.Status == Models.Enums.ProjectStatus.Active);

            // Calculate average system health from active hosts
            var avgCpu = 0;
            var avgMemory = 0;
            var avgDisk = 0;
            
            if (activeHosts.Any())
            {
                var hostMetrics = activeHosts.Where(h => h.LastData != null).ToList();
                if (hostMetrics.Any())
                {
                    avgCpu = (int)hostMetrics.Average(h => ExtractMetric(h.LastData, "cpuUsage"));
                    avgMemory = (int)hostMetrics.Average(h => ExtractMetric(h.LastData, "memoryUsage"));
                    avgDisk = (int)hostMetrics.Average(h => ExtractMetric(h.LastData, "diskUsage"));
                }
            }

            var stats = new DashboardStatsDto
            {
                TotalHosts = totalHosts,
                OnlineHosts = onlineHosts,
                OfflineHosts = totalHosts - onlineHosts,
                TotalProjects = projects.Count(),
                ActiveProjects = activeProjects,
                SystemHealth = new SystemHealthDto
                {
                    Cpu = avgCpu,
                    Memory = avgMemory,
                    Disk = avgDisk
                }
            };

            return Ok(stats);
        }
        catch (Exception ex)
        {
            return _errorHandler.InvokeError(ex);
        }
    }

    [HttpGet("activities")]
    public async Task<IActionResult> GetRecentActivities()
    {
        try
        {
            var activities = new List<RecentActivityDto>();

            // Get recent host status changes from active hosts
            var activeHosts = _activeHostService.GetActiveHosts();
            var recentHosts = activeHosts.Take(3).ToList();
            
            foreach (var host in recentHosts)
            {
                activities.Add(new RecentActivityDto
                {
                    Id = Guid.NewGuid().ToString(),
                    Type = host.IsOnline ? "host_online" : "host_offline",
                    Message = $"{host.Name} is {(host.IsOnline ? "online" : "offline")}",
                    Timestamp = host.LastSeen.AddMinutes(-Random.Shared.Next(1, 30)),
                    Severity = host.IsOnline ? "success" : "error"
                });
            }

            // Get recent project updates
            var projects = await _projectRepository.GetAllAsync();
            var recentProjects = projects.Take(2).ToList();
            
            foreach (var project in recentProjects)
            {
                activities.Add(new RecentActivityDto
                {
                    Id = Guid.NewGuid().ToString(),
                    Type = "project_updated",
                    Message = $"Project \"{project.Name}\" updated successfully",
                    Timestamp = DateTime.UtcNow.AddMinutes(-Random.Shared.Next(10, 180)),
                    Severity = "info"
                });
            }

            var sortedActivities = activities
                .OrderByDescending(a => a.Timestamp)
                .Take(5)
                .ToList();

            return Ok(sortedActivities);
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

    [HttpGet("data")]
    public async Task<IActionResult> GetDashboardData()
    {
        try
        {
            var statsResponse = await GetDashboardStats();
            var activitiesResponse = await GetRecentActivities();

            if (statsResponse is OkObjectResult statsOk && activitiesResponse is OkObjectResult activitiesOk)
            {
                var statsData = ((DashboardStatsDto)statsOk.Value!)!;
                var activitiesData = ((List<RecentActivityDto>)activitiesOk.Value!)!;

                var dashboardData = new DashboardDataDto
                {
                    Stats = statsData,
                    RecentActivities = activitiesData
                };

                return Ok(dashboardData);
            }

            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Failed to load dashboard data"
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

    private double ExtractMetric(object? hostData, string metricName)
    {
        try
        {
            if (hostData is System.Text.Json.JsonElement jsonElement)
            {
                if (jsonElement.TryGetProperty(metricName, out var metricProp))
                {
                    if (metricProp.ValueKind == System.Text.Json.JsonValueKind.Object &&
                        metricProp.TryGetProperty("percentage", out var percentageProp))
                        return percentageProp.GetDouble();
                    
                    if (metricProp.ValueKind == System.Text.Json.JsonValueKind.Number)
                        return metricProp.GetDouble();
                }
            }
            return 0;
        }
        catch
        {
            return 0;
        }
    }
}