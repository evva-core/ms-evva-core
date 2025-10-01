using ms_evva_core.Base.Attributes;
using System;

namespace ms_evva_core.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        [Column("user_id")]
        public int? UserId { get; set; }
        public string Action { get; set; }
        public string Details { get; set; } // JSON stored as string
        [Column("ip_address")]
        public string IpAddress { get; set; }
        public DateTime Timestamp { get; set; }
    }
}