using ms_evva_core.Base;
using ms_evva_core.Models;
using ms_evva_core.Repos.Interfaces;

namespace ms_evva_core.Repos.Classes;

public class HostRepository : GenericRepository<Models.Host>, IHostRepository
{
    public HostRepository() : base("hosts")
    {
    }

    public async Task<Models.Host?> GetByUniqueIdAsync(string uniqueId)
    {
        return await FindFirstOrDefaultAsync("uniqueid = @UniqueId", new { UniqueId = uniqueId });
    }
}