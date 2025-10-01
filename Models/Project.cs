using ms_evva_core.Base.Attributes;
using ms_evva_core.Models.Enums;
using System;

namespace ms_evva_core.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }
        [Column("status")]
        public ProjectStatus? Status { get; set; }
        [Column("owner_id")]
        public int OwnerId { get; set; }
        
    }
}
