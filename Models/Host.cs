using ms_evva_core.Base.Attributes;

namespace ms_evva_core.Models
{
    public class Host
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [Column("operating_system")]
        public string OperatingSystem { get; set; }
        [Column("ip_address")]
        public string? IpAddress { get; set; } = string.Empty;
        public string Architecture { get; set; } = string.Empty;
        [Column("is_active")]
        public bool IsActive { get; set; } = true;
        public int Port { get; set; } = 5643;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        [Column("unique_id")]
        public string UniqueId { get; set; } = Guid.NewGuid().ToString();
    }
}
