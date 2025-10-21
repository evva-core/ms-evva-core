using Dapper;
using ms_evva_core.Base;
using ms_evva_core.Models;
using ms_evva_core.Repos.Interfaces;
using ms_evva_core.Utils;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace ms_evva_core.Repos.Classes;

public class ProjectWorkflowRepository : GenericRepository<ProjectWorkflow>, IProjectWorkflowRepository
{
    public ProjectWorkflowRepository( ) : base("project_workflow")
    {
    }

    protected override string SelectColumns => string.Join(", ", 
        _entityProperties
            .Where(p => !p.GetCustomAttributes<NotMappedAttribute>().Any())
            .Select(p => $"{GetColumnName(p)} AS {p.Name}"));

    public async Task<IEnumerable<ProjectWorkflow>> GetByProjectIdAsync(int projectId)
    {
        using var conn = await _connectionProvider.CreateConnectionAsync();
        string sql = @"SELECT 
            pw.id AS Id,
            pw.project_id AS ProjectId,
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

        var result = await conn.QueryAsync<ProjectWorkflow, Workflow, ProjectWorkflow>(
            sql,
            (pw, w) => {
                pw.Workflow = w;
                return pw;
            },
            new { ProjectId = projectId },
            splitOn: "Id"
        );

        return result;
    }

    public override async Task<int> AddAsync(ProjectWorkflow entity)
    {
        using var conn = await _connectionProvider.CreateConnectionAsync();
        string sql = @"INSERT INTO project_workflow (project_id, workflow_id, execution_order, stage_name) 
                      VALUES (@ProjectId, @WorkflowId, @ExecutionOrder, @StageName) 
                      RETURNING id";
        
        return await conn.ExecuteScalarAsync<int>(sql, new {
            ProjectId = entity.ProjectId,
            WorkflowId = entity.WorkflowId,
            ExecutionOrder = entity.ExecutionOrder,
            StageName = entity.StageName
        });
    }

    public override async Task<bool> UpdateAsync(ProjectWorkflow entity)
    {
        using var conn = await _connectionProvider.CreateConnectionAsync();
        string sql = @"UPDATE project_workflow 
                      SET project_id = @ProjectId, workflow_id = @WorkflowId, 
                          execution_order = @ExecutionOrder, stage_name = @StageName 
                      WHERE id = @Id";
        
        var rowsAffected = await conn.ExecuteAsync(sql, new {
            Id = entity.Id,
            ProjectId = entity.ProjectId,
            WorkflowId = entity.WorkflowId,
            ExecutionOrder = entity.ExecutionOrder,
            StageName = entity.StageName
        });
        
        return rowsAffected > 0;
    }
}