using NpgsqlTypes;

namespace ms_evva_core.Models.Enums
{
    [PgName("supported_os")]
    public enum SupportedOs
    {
        [PgName("linux")]
        Linux,
        [PgName("windows")]
        Windows,
        [PgName("macos")]
        MacOs,
        [PgName("any")]
        Any
    }
}
