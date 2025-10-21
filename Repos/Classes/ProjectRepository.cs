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
        using var conn = await _connectionProvider.CreateConnectionAsync();
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
        using var conn = await _connectionProvider.CreateConnectionAsync();
        
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
        
        return project!;
    }

    public async Task<IEnumerable<ProjectWorkflow>> GetProjectWorkflowsAsync(int projectId)
    {
        using var conn = await _connectionProvider.CreateConnectionAsync();
        
        string sql = @"SELECT 
            pw.id AS Id,
            pw.workflow_id AS WorkflowId,
            pw.execution_order AS ExecutionOrder,
            pw.stage_name AS StageName,
            w.id AS Id,
            w.name AS Name,
            w.description AS Description,
            w.command AS Command,
            w.supported_os AS SupportedOs,
            w.parameters AS Parameters,
            w.is_json_required AS IsJsonRequired,
            w.json_data AS JsonData
        FROM project_workflow pw
        INNER JOIN workflows w ON pw.workflow_id = w.id
        WHERE pw.project_id = @ProjectId
        ORDER BY pw.execution_order";
        
        var workflowDict = new Dictionary<int, ProjectWorkflow>();
        
        await conn.QueryAsync<ProjectWorkflow, Workflow, ProjectWorkflow>(
            sql,
            (projectWorkflow, workflow) =>
            {
                if (!workflowDict.TryGetValue(projectWorkflow.Id, out var existingWorkflow))
                {
                    projectWorkflow.Workflow = workflow;
                    workflowDict.Add(projectWorkflow.Id, projectWorkflow);
                    return projectWorkflow;
                }
                return existingWorkflow;
            },
            new { ProjectId = projectId },
            splitOn: "Id"
        );
        
        return workflowDict.Values;
    }
}