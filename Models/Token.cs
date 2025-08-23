namespace ms_evva_core.Models
{
    public class Token
    {
        public int Id { get; set; }
        public int? HostId { get; set; }
        public int? UserId { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
        public DateTime? ExpiresIn { get; set; } = null;
        public string Hash { get; } = Guid.NewGuid().ToString();
        public string Type { get; set; } = string.Empty;
    }
} 