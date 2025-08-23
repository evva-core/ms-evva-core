using ms_evva_core.Repos.Interfaces;
using ms_evva_core.Models;

namespace ms_evva_core.Repos.Interfaces;

public interface ITokenRepository : IRepository<Models.Token>
{
    Task<Token> GetByHash(string hash);
} 