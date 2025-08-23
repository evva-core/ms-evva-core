namespace ms_evva_core.Models
{
    public class HostMetric
    {
        public int Id { get; set; }
        public int HostId { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal? Cpu_usage { get; set; }
        public int? Memory_usage_mb { get; set; }
        public decimal? Disk_usage_gb { get; set; }
        public decimal? Network_in_mb { get; set; }
        public decimal? Network_out_mb { get; set; }
    }
}
