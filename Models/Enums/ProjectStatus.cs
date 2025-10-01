using System.ComponentModel.DataAnnotations;
using NpgsqlTypes;

namespace ms_evva_core.Models.Enums
{
    [PgName("project_status")]
    public enum ProjectStatus
    {
        [PgName("active")]
        Active,
        [PgName("archived")]
        Archived,
        [PgName("online")]
        Online
    }
}
