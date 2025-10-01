namespace ms_evva_core.Models.Dtos;

public class ProjectDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public string OwnerName { get; set; }
    public int TotalRepos { get; set; }
}
