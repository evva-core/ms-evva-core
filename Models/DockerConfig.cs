using ms_evva_core.Base.Attributes;

namespace ms_evva_core.Models
{
    public class DockerConfig
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        [Column("host_ports")]
        public string HostPorts { get; set; } = string.Empty;
        [Column("run_params")]
        public string? RunParams { get; set; }
        [Column("container_name")]
        public string ContainerName { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        [Column("env_vars")]
        public string? EnvVars { get; set; }
        public string? Volumes { get; set; }
    }
}