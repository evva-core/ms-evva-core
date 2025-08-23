namespace ms_evva_core.Models;

public class ApiResponse<T>
{
    public T Data { get; set; }
    public bool Success { get; set; } = true;
    public string? Message { get; set; }
    public Dictionary<string, string[]>? Errors { get; set; }
}
