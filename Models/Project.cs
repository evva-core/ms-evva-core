namespace ms_evva_core.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? Status { get; set; }
        public int OwnerId { get; set; }
    }
}