using ms_evva_core.Models.Dtos;
using ms_evva_core.Services.Interfaces;
using System.Collections.Concurrent;

namespace ms_evva_core.Services.Classes;

public class ActiveHostService : IActiveHostService
{
    private readonly ConcurrentDictionary<string, ActiveHostDto> _activeHosts = new();
    private readonly ILogger<ActiveHostService> _logger;

    public ActiveHostService(ILogger<ActiveHostService> logger)
    {
        _logger = logger;
    }

    public void UpdateHostActivity(string uniqueId, object hostData)
    {
        var now = DateTime.UtcNow;
        var expiresAt = now.AddMinutes(5);

        _activeHosts.AddOrUpdate(uniqueId, 
            new ActiveHostDto
            {
                UniqueId = uniqueId,
                Name = ExtractHostName(hostData),
                LastSeen = now,
                ExpiresAt = expiresAt,
                LastData = hostData
            },
            (key, existing) =>
            {
                existing.LastSeen = now;
                existing.ExpiresAt = expiresAt;
                existing.LastData = hostData;
                existing.Name = ExtractHostName(hostData);
                return existing;
            });

        _logger.LogDebug("Updated host activity for {UniqueId}, expires at {ExpiresAt}", uniqueId, expiresAt);
    }

    public List<ActiveHostDto> GetActiveHosts()
    {
        RemoveExpiredHosts();
        return _activeHosts.Values.Where(h => h.IsOnline).ToList();
    }

    public void RemoveExpiredHosts()
    {
        var now = DateTime.UtcNow;
        var expiredHosts = _activeHosts.Where(kvp => kvp.Value.ExpiresAt <= now).ToList();

        foreach (var expired in expiredHosts)
        {
            _activeHosts.TryRemove(expired.Key, out _);
            _logger.LogInformation("Removed expired host {UniqueId}", expired.Key);
        }
    }

    public int GetOnlineHostsCount()
    {
        RemoveExpiredHosts();
        return _activeHosts.Count(kvp => kvp.Value.IsOnline);
    }

    public int GetTotalActiveHostsCount()
    {
        RemoveExpiredHosts();
        return _activeHosts.Count;
    }

    private string ExtractHostName(object hostData)
    {
        try
        {
            if (hostData is System.Text.Json.JsonElement jsonElement)
            {
                if (jsonElement.TryGetProperty("hostName", out var hostNameProp))
                    return hostNameProp.GetString() ?? "Unknown Host";
                if (jsonElement.TryGetProperty("name", out var nameProp))
                    return nameProp.GetString() ?? "Unknown Host";
            }
            return "Unknown Host";
        }
        catch
        {
            return "Unknown Host";
        }
    }
}