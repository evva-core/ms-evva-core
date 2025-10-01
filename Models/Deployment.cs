using ms_evva_core.Base.Attributes;
using ms_evva_core.Models.Enums;
using System;

namespace ms_evva_core.Models
{
    public class Deployment
    {
        public int Id { get; set; }
        [Column("repository_id")]
        public int RepositoryId { get; set; }
        [Column("workflow_id")]
        public int? WorkflowId { get; set; }
        [Column("status")]
        public DeploymentStatus Status { get; set; }
        [Column("started_at")]
        public DateTime? StartedAt { get; set; }
        [Column("finished_at")]
        public DateTime? FinishedAt { get; set; }
        [Column("triggered_by")]
        public int TriggeredBy { get; set; }
        [Column("error_message")]
        public string ErrorMessage { get; set; }
    }
}