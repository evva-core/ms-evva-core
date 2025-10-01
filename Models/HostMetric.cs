using ms_evva_core.Base.Attributes;
using System;

namespace ms_evva_core.Models
{
    public class HostMetric
    {
        public int Id { get; set; }
        [Column("host_id")]
        public int HostId { get; set; }
        public DateTime Timestamp { get; set; }
        [Column("cpu_usage")]
        public decimal? CpuUsage { get; set; }
        [Column("memory_usage_mb")]
        public int? MemoryUsageMb { get; set; }
        [Column("disk_usage_gb")]
        public decimal? DiskUsageGb { get; set; }
        [Column("network_in_mb")]
        public decimal? NetworkInMb { get; set; }
        [Column("network_out_mb")]
        public decimal? NetworkOutMb { get; set; }
    }
}