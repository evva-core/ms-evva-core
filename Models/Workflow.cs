using System.Reflection.Metadata.Ecma335;
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
        [Column("supported_os")]
        public SupportedOs SupportedOs { get; set; }
        public string? Parameters { get; set; }
        [Column("is_json_required")]
        public bool IsJsonRequired { get; set; }
         [Column("json_data")]
        public string? JsonData { get; set; }

    }
}