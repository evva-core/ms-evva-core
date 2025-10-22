namespace ms_evva_core.Models.Dtos;

public class ActiveHostDto
{
    public string UniqueId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTime LastSeen { get; set; }
    public DateTime ExpiresAt { get; set; }
    public object? LastData { get; set; }
    public bool IsOnline => DateTime.UtcNow < ExpiresAt;
}