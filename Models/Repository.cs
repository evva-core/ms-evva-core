using ms_evva_core.Base.Attributes;
using ms_evva_core.Models.Enums;
using System;

namespace ms_evva_core.Models
{
    public class Repository
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Column("project_id")]
        public int ProjectId { get; set; }
        [Column("repository_url")]
        public string RepositoryUrl { get; set; }
        public string Branch { get; set; }
        [Column("target_path")]
        public string TargetPath { get; set; }
        [Column("host_id")]
        public int HostId { get; set; }
        [Column("status")]
        public RepositoryStatus Status { get; set; }
        [Column("last_sync")]
        public DateTime? LastSync { get; set; }
        [Column("last_clone")]
        public DateTime? LastClone { get; set; }
        [Column("last_push")]
        public DateTime? LastPush { get; set; }
        [Column("last_commit_hash")]
        public string? LastCommitHash { get; set; }
        [Column("auto_sync")]
        public bool? AutoSync { get; set; }
        [Column("is_docker_enabled")]
        public bool? IsDockerEnabled { get; set; }
        [Column("docker_config_id")]
        public int? DockerConfigId { get; set; }
    }
}