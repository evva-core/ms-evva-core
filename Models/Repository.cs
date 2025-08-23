namespace ms_evva_core.Models
{
    public class Repository
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int SolutionId { get; set; }
        public string RepositoryUrl { get; set; }
        public string Branch { get; set; }
        public string TargetPath { get; set; }
        public int HostId { get; set; }
        public int Status { get; set; }
        public DateTime? LastSync { get; set; }
        public DateTime? LastClone { get; set; }
        public DateTime? LastPush { get; set; }
        public string Last_commit_hash { get; set; }
        public bool? AutoSync { get; set; }
        public bool? IsDockerEnabled { get; set; }
        public int DockerConfigId { get; set; }
    }
}
