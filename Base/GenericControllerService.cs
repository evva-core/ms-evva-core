using Microsoft.AspNetCore.Mvc;
using ms_evva_core.Repos.Interfaces;
using ms_evva_core.Services.Interfaces;
using ms_evva_core.Utils;

namespace ms_evva_core.Base;

public abstract class GenericControllerService<T> : ControllerBase,IControllerService<T> where T : class
{
    protected readonly IRepository<T> Repository;
    protected readonly ErrorHandler ErrorHandler = new();

    protected GenericControllerService(IRepository<T> repository)
    {
        Repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public virtual async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var result = await Repository.GetAllAsync();
            return Ok(new Models.ApiResponse<IEnumerable<T>>
            {
                Data = result,
                Success = true
            });
        }
        catch (Exception ex)
        {
            return ErrorHandler.InvokeError(ex);
        }
    }

    public virtual async Task<IActionResult> GetByIdAsync(int id)
    {
        try
        {
            var result = await Repository.GetByIdAsync(id);
            if (result != null)
            {
                return Ok(new Models.ApiResponse<T>
                {
                    Data = result,
                    Success = true
                });
            }
            return NotFound(new Models.ApiResponse<T>
            {
                Success = false,
                Message = "Entity not found"
            });
        }
        catch (Exception ex)
        {
            return ErrorHandler.InvokeError(ex);
        }
    }

    public virtual async Task<IActionResult> AddAsync(T entity)
    {
        try
        {
            var id = await Repository.AddAsync(entity);
            return Ok(id);
        }
        catch (Exception ex)
        {
            return ErrorHandler.InvokeError(ex);
        }
    }

    public virtual async Task<IActionResult> UpdateAsync(T entity)
    {
        try
        {
            var success = await Repository.UpdateAsync(entity);
            return success ? Ok() : NotFound();
        }
        catch (Exception ex)
        {
            return ErrorHandler.InvokeError(ex);
        }
    }

    public virtual async Task<IActionResult> DeleteAsync(int id)
    {
        try
        {
            var success = await Repository.DeleteAsync(id);
            return success ? Ok() : NotFound();
        }
        catch (Exception ex)
        {
            return ErrorHandler.InvokeError(ex);
        }
    }
}