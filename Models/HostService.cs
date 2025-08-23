namespace ms_evva_core.Models
{
    public class HostService
    {
        public int Id { get; set; }
        public int HostId { get; set; }
        public string Service_name { get; set; }
        public int? Port { get; set; }
        public string Status { get; set; }
        public DateTime? Last_check { get; set; }
        public string Path { get; set; }
        public string Workdir { get; set; }
    }
}
