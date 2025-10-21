namespace ms_evva_core.Models.Dtos;

public class DeploymentRequestDto
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public int HostId { get; set; }
    public string HostUniqueId { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public List<RepositoryDto> Repositories { get; set; } = new();
    public List<WorkflowStepDto> WorkflowSteps { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}

public class RepositoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string RepositoryUrl { get; set; } = string.Empty;
    public string Branch { get; set; } = string.Empty;
    public string TargetPath { get; set; } = string.Empty;
    public bool IsDockerEnabled { get; set; }
    public int? DockerConfigId { get; set; }
}

public class WorkflowStepDto
{
    public int Id { get; set; }
    public int ExecutionOrder { get; set; }
    public string StageName { get; set; } = string.Empty;
    public string WorkflowName { get; set; } = string.Empty;
    public string Command { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string SupportedOs { get; set; } = string.Empty;
    public string? Parameters { get; set; }
    public bool IsJsonRequired { get; set; }
    public string? JsonData { get; set; }
}