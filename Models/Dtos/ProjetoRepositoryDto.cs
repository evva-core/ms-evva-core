namespace ms_evva_core.Models.Dtos;

public class ProjetoRepositoryDto: Project
{
    public List<Repository> Repositories { get; set; }
}