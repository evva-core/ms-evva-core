using ms_evva_core.Base.Attributes;

namespace ms_evva_core.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        [Column("is_admin")]
        public bool IsAdmin { get; set; } = false;
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Column("is_active")]
        public bool IsActive { get; set; } = true;
    }
}