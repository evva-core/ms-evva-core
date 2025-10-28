using ms_evva_core.Base;
using ms_evva_core.Models;
using ms_evva_core.Repos.Interfaces;
using ms_evva_core.Utils;
using Dapper;

namespace ms_evva_core.Repos.Classes;

public class WorkflowRepository : GenericRepository<Workflow>, IWorkflowRepository
{
    public WorkflowRepository() : base("workflows")
    {
    }

    public override async Task<int> AddAsync(Workflow entity)
    {
        var sql = $@"
            INSERT INTO {TableName} (name, command, description, supported_os, parameters, is_json_required, json_data)
            VALUES (@Name, @Command, @Description, @SupportedOsString::supported_os, @Parameters, @IsJsonRequired, @JsonData::json)
            RETURNING id";

        using var connection = new ConnectionProvider().CreateConnection();
        var parameters = new
        {
            entity.Name,
            entity.Command,
            entity.Description,
            SupportedOsString = entity.SupportedOs.ToString().ToLower(),
            entity.Parameters,
            entity.IsJsonRequired,
            entity.JsonData
        };
        return await connection.QuerySingleAsync<int>(sql, parameters);
    }

    public override async Task<bool> UpdateAsync(Workflow entity)
    {
        var sql = $@"
            UPDATE {TableName} 
            SET name = @Name, command = @Command, description = @Description, 
                supported_os = @SupportedOsString::supported_os, parameters = @Parameters, 
                is_json_required = @IsJsonRequired, json_data = @JsonData::json
            WHERE id = @Id";

        using var connection = new ConnectionProvider().CreateConnection();
        var parameters = new
        {
            entity.Name,
            entity.Command,
            entity.Description,
            SupportedOsString = entity.SupportedOs.ToString().ToLower(),
            entity.Parameters,
            entity.IsJsonRequired,
            entity.JsonData,
            entity.Id
        };
        var rowsAffected = await connection.ExecuteAsync(sql, parameters);
        return rowsAffected > 0;
    }

    public async Task<IEnumerable<Workflow>> GetAvailableWorkflowsAsync()
    {
        var sql = @"
    SELECT w.name,
    w.id,
	w.command,
	w.description,
	w.is_json_required as IsJsonRequired,
	w.supported_os,
	w.json_data as JsonData FROM workflows w
            WHERE w.id NOT IN (
                SELECT DISTINCT workflow_id FROM project_workflow
            )
            ORDER BY w.name";

        using var connection = new ConnectionProvider().CreateConnection();
        return await connection.QueryAsync<Workflow>(sql);
    }
}