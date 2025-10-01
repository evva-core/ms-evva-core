using Dapper;
using ms_evva_core.Base;
using ms_evva_core.Models;
using ms_evva_core.Models.Dtos;
using ms_evva_core.Repos.Interfaces;
using ms_evva_core.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace ms_evva_core.Repos.Classes;

public class ProjectRepository : GenericRepository<Project>, IProjectRepository
{
    public ProjectRepository() : base("projects")
    {
    }

    public async Task<IEnumerable<ProjectDto>> GetAllProjectsWithDetailsAsync()
    {
        var conn = await _connectionProvider.CreateConnectionAsync();
        string sql = $@"SELECT 
    proj.id AS Id,
    proj.name AS Name,
    proj.description AS Description,
    proj.created_at AS CreatedAt,
    usr.name AS OwnerName,
    COUNT(repo.id) AS TotalRepos
FROM projects AS proj
INNER JOIN users AS usr 
    ON proj.owner_id = usr.id
LEFT JOIN repositories AS repo
    ON proj.id = repo.project_id
GROUP BY 
    proj.id,
    proj.name,
    proj.description,
    proj.created_at,
    usr.name;
";

        return await conn.QueryAsync<ProjectDto>(sql);
    }

    public async Task<ProjectDetailsDto> GetProjectWithDetailsAsync(int id)
    {
        var conn = await _connectionProvider.CreateConnectionAsync();
        
        string projectSql = @"SELECT 
            proj.id AS Id,
            proj.name AS Name,
            proj.description AS Description,
            proj.created_at AS CreatedAt,
            proj.status AS Status,
            proj.owner_id AS OwnerId,
            usr.name AS OwnerName
        FROM projects AS proj
        INNER JOIN users AS usr ON proj.owner_id = usr.id
        WHERE proj.id = @Id";
        
        string repositoriesSql = @"SELECT 
            id AS Id,
            name AS Name,
            project_id AS ProjectId,
            repository_url AS RepositoryUrl,
            branch AS Branch,
            target_path AS TargetPath,
            host_id AS HostId,
            status AS Status,
            last_sync AS LastSync,
            last_clone AS LastClone,
            last_push AS LastPush,
            last_commit_hash AS LastCommitHash,
            auto_sync AS AutoSync,
            is_docker_enabled AS IsDockerEnabled,
            docker_config_id AS DockerConfigId
        FROM repositories 
        WHERE project_id = @Id";
        
        var project = await conn.QueryFirstOrDefaultAsync<ProjectDetailsDto>(projectSql, new { Id = id });
        if (project != null)
        {
            project.Repositories = (await conn.QueryAsync<Repository>(repositoriesSql, new { Id = id })).ToList();
        }
        
        return project;
    }
}