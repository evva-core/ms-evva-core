using ms_evva_core.Base.Attributes;
using ms_evva_core.Models.Enums;

namespace ms_evva_core.Models
{
    public class Workflow
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Command { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Column("icon_url")]
        public string? IconUrl { get; set; }
        [Column("supported_os")]
        public SupportedOs SupportedOs { get; set; }
    }
}