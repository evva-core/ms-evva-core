namespace ms_evva_core.Models
{
    public class Deployment
    {
        public int Id { get; set; }
        public int RepositoryId { get; set; }
        public int? WorkflowId { get; set; }
        public string Status { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public int TriggeredBy { get; set; }
    }
}
