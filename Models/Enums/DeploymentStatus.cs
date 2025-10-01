using NpgsqlTypes;

namespace ms_evva_core.Models.Enums
{
    [PgName("deployment_status")]
    public enum DeploymentStatus
    {
        [PgName("pending")]
        Pending,
        [PgName("running")]
        Running,
        [PgName("success")]
        Success,
        [PgName("failed")]
        Failed,
        [PgName("cancelled")]
        Cancelled
    }
}
