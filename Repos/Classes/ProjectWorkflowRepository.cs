using Dapper;
using ms_evva_core.Base;
using ms_evva_core.Models;
using ms_evva_core.Repos.Interfaces;
using ms_evva_core.Utils;

namespace ms_evva_core.Repos.Classes;

public class ProjectWorkflowRepository : GenericRepository<ProjectWorkflow>, IProjectWorkflowRepository
{
    public ProjectWorkflowRepository( ) : base("project_workflow")
    {
    }

    public async Task<IEnumerable<ProjectWorkflow>> GetByProjectIdAsync(int projectId)
    {
        var conn = await _connectionProvider.CreateConnectionAsync();
        string sql = @"SELECT 
            pw.id AS Id,
            pw.project_id AS ProjectId,
            pw.workflow_id AS WorkflowId,
            pw.execution_order AS ExecutionOrder,
            pw.stage_name AS StageName,
            w.id AS Id,
            w.name AS Name,
            w.description AS Description,
            w.command AS Command
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
}