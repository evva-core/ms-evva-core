namespace ms_evva_core.Models.Dtos;

public class DashboardStatsDto
{
    public int TotalHosts { get; set; }
    public int OnlineHosts { get; set; }
    public int OfflineHosts { get; set; }
    public int TotalProjects { get; set; }
    public int ActiveProjects { get; set; }
    public SystemHealthDto SystemHealth { get; set; } = new();
}

public class SystemHealthDto
{
    public int Cpu { get; set; }
    public int Memory { get; set; }
    public int Disk { get; set; }
}

public class RecentActivityDto
{
    public string Id { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string Severity { get; set; } = string.Empty;
}

public class DashboardDataDto
{
    public DashboardStatsDto Stats { get; set; } = new();
    public List<RecentActivityDto> RecentActivities { get; set; } = new();
}