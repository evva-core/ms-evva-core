using ms_evva_core.Repos.Interfaces;
using ms_evva_core.Models;

namespace ms_evva_core.Repos.Interfaces;

public interface IHostRepository : IRepository<Models.Host>
{
    Task<Models.Host?> GetByUniqueIdAsync(string uniqueId);
// Adicionar métodos adicionais aqui
}