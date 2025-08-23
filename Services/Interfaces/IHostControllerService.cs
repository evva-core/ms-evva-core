using ms_evva_core.Services.Interfaces;
using ms_evva_core.Models;

namespace ms_evva_core.Services.Interfaces;

public interface IHostControllerService : IControllerService<Models.Host>
{

    Task<Models.Host?> GetHostByUniqueIdAsync(string uniqueId);
}