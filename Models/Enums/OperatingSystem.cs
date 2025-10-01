using NpgsqlTypes;

namespace ms_evva_core.Models.Enums
{
    [PgName("operating_system")]
    public enum OperatingSystem
    {
        [PgName("windows")]
        Windows,
        [PgName("linux")]
        Linux,
        [PgName("macos")]
        MacOS
    }
}