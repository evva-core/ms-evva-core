using ms_evva_core.Base;
using ms_evva_core.Models;
using ms_evva_core.Repos.Interfaces;
using ms_evva_core.Services.Interfaces;

namespace ms_evva_core.Services.Classes;

public class TokenControllerService : GenericControllerService<Token>, ITokenControllerService
{
    private readonly ITokenRepository _tokenRepository;

    public TokenControllerService(ITokenRepository repository) : base(repository)
    {
        _tokenRepository = repository;
    }

    public async Task<Token> GetByHash(string hash)
    {
        try
        {
            return await _tokenRepository.GetByHash(hash);
        }
        catch (Exception)
        {
            throw;
        }
    }
} 