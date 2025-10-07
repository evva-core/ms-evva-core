using ms_evva_core.Base.Attributes;

namespace ms_evva_core.Models
{
    public class ProjectWorkflow
    {
        public int Id { get; set; }
        [Column("project_id")]
        public int ProjectId { get; set; }
        [Column("workflow_id")]
        public int WorkflowId { get; set; }
        [Column("execution_order")]
        public int ExecutionOrder { get; set; }
        [Column("stage_name")]
        public string? StageName { get; set; }
        public Workflow? Workflow { get; set; }
    }
}