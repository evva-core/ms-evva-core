namespace ms_evva_core.Models
{
    public class Workflow
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Command { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? IconUrl { get; set; }
        public bool IsLinux { get; set; }
    }
} 