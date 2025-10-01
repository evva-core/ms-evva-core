using ms_evva_core.Models.Dtos;
using ms_evva_core.Repos.Interfaces;
using ms_evva_core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ms_evva_core.Repos.Interfaces;

public interface IProjectRepository : IRepository<Models.Project>
{
    Task<IEnumerable<ProjectDto>> GetAllProjectsWithDetailsAsync();
    Task<ProjectDetailsDto> GetProjectWithDetailsAsync(int id);
}