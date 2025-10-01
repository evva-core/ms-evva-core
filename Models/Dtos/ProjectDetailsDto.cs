using ms_evva_core.Models.Enums;

namespace ms_evva_core.Models.Dtos;

public class ProjectDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public ProjectStatus Status { get; set; }
    public int OwnerId { get; set; }
    public string OwnerName { get; set; }
    public List<Repository> Repositories { get; set; } = new List<Repository>();
}