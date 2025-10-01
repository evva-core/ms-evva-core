using NpgsqlTypes;

namespace ms_evva_core.Models.Enums
{
    [PgName("service_status")]
    public enum ServiceStatus
    {
        [PgName("running")]
        Running,
        [PgName("stopped")]
        Stopped,
        [PgName("error")]
        Error,
        [PgName("unknown")]
        Unknown
    }
}
