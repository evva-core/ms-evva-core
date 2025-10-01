using Dapper;
using ms_evva_core.Base;
using ms_evva_core.Models;
using ms_evva_core.Repos.Interfaces;
using ms_evva_core.Utils;

namespace ms_evva_core.Repos.Classes;

public class TokenRepository : GenericRepository<Token>, ITokenRepository
{
    public TokenRepository() : base("tokens")
    {
    }

    public async Task<Token> GetByHash(string hash)
    {
        var db = await  _connectionProvider.CreateConnectionAsync();
        return await db.QueryFirstOrDefaultAsync<Token>("SELECT * FROM tokens WHERE hash = @Hash", new { Hash = hash })??throw new ArgumentOutOfRangeException(hash);
    }
}