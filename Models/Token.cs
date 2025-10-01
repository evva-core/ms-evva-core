using ms_evva_core.Base.Attributes;
using System;

namespace ms_evva_core.Models
{
    public class Token
    {
        public int Id { get; set; }
        [Column("host_id")]
        public int? HostId { get; set; }
        [Column("user_id")]
        public int? UserId { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Column("is_active")]
        public bool IsActive { get; set; } = true;
        [Column("expires_at")]
        public DateTime? ExpiresAt { get; set; } = null;
        public string Hash { get; } = Guid.NewGuid().ToString();
        public string Type { get; set; } = string.Empty;
    }
}