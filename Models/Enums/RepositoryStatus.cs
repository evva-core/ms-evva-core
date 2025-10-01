using NpgsqlTypes;

namespace ms_evva_core.Models.Enums
{
    [PgName("repository_status")]
    public enum RepositoryStatus
    {
        [PgName("active")]
        Active,
        [PgName("syncing")]
        Syncing,
        [PgName("error")]
        Error,
        [PgName("inactive")]
        Inactive
    }
}
