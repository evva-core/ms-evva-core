using Dapper;
using ms_evva_core.Base;
using ms_evva_core.Models;
using ms_evva_core.Repos.Interfaces;
using ms_evva_core.Utils;

namespace ms_evva_core.Repos.Classes
{
    public class RepositoryRepository : GenericRepository<Repository>, IRepositoryRepository
    {
        public RepositoryRepository() : base("repositories")
        {
        }

        public async Task<IEnumerable<Repository>> GetByProjectIdAsync(int projectId)
        {
            using var conn = await _connectionProvider.CreateConnectionAsync();
            string sql = @"SELECT 
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
            WHERE project_id = @ProjectId";
            
            return await conn.QueryAsync<Repository>(sql, new { ProjectId = projectId });
        }
    }
}