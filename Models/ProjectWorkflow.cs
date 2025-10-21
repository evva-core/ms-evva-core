using System.ComponentModel.DataAnnotations.Schema;
using ms_evva_core.Base.Attributes;
using ColumnAttribute = ms_evva_core.Base.Attributes.ColumnAttribute;

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
        [Base.Attributes.Column("stage_name")]
        public string? StageName { get; set; }
        [NotMapped]
        public Workflow? Workflow { get; set; }
    }
}