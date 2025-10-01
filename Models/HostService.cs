using ms_evva_core.Base.Attributes;
using ms_evva_core.Models.Enums;
using System;

namespace ms_evva_core.Models
{
    public class HostService
    {
        public int Id { get; set; }
        [Column("host_id")]
        public int HostId { get; set; }
        [Column("service_name")]
        public string ServiceName { get; set; }
        public int? Port { get; set; }
        [Column("status")]
        public ServiceStatus Status { get; set; }
        [Column("last_check")]
        public DateTime? LastCheck { get; set; }
        public string Path { get; set; }
        [Column("work_dir")]
        public string WorkDir { get; set; }
    }
}