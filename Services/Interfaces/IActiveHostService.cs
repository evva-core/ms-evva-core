using ms_evva_core.Models.Dtos;

namespace ms_evva_core.Services.Interfaces;

public interface IActiveHostService
{
    void UpdateHostActivity(string uniqueId, object hostData);
    List<ActiveHostDto> GetActiveHosts();
    void RemoveExpiredHosts();
    int GetOnlineHostsCount();
    int GetTotalActiveHostsCount();
}