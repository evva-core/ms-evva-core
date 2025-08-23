using Microsoft.AspNetCore.Mvc;
using ms_evva_core.Base;
using ms_evva_core.Models;
using ms_evva_core.Repos.Interfaces;
using ms_evva_core.Services.Interfaces;
using ms_evva_core.Utils;

namespace ms_evva_core.Services.Classes;

public class HostControllerService : GenericControllerService<Models.Host>, IHostControllerService
{
    private readonly IHostRepository _hostRepository;
    private readonly ITokenRepository _tokenRepository;
    private ErrorHandler errorHandler = new();


    public HostControllerService(IHostRepository hostRepository, ITokenRepository tokenRepository)
        : base(hostRepository)
    {
        _hostRepository = hostRepository;
        _tokenRepository = tokenRepository;
    }

    public async Task<Models.Host?> GetHostByUniqueIdAsync(string uniqueId)
    {
        return await _hostRepository.GetByUniqueIdAsync(uniqueId);
    }

    public async Task<IActionResult> RegisterHost(string hash, Models.Host host)
    {
        try
        {
            Token token = await _tokenRepository.GetByHash(hash);
            if (token.ExpiresIn < DateTime.Now)
            {
                token.IsActive = false;
                await _tokenRepository.UpdateAsync(token);
            }
            return Ok(await _hostRepository.AddAsync(host));
           
        }
        catch (Exception ex)
        {
            return ErrorHandler.InvokeError(ex);
        }
    }
}