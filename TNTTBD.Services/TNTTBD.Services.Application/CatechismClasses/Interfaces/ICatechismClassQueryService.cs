using TNTTBD.Services.Application.CatechismClasses.DTOs;

namespace TNTTBD.Services.Application.CatechismClasses.Interfaces;

public interface ICatechismClassQueryService
{
    /// <summary>
    /// Get class by id
    /// </summary>
    Task<CatechismClassDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Get list of class
    /// </summary>
    /// <param name="yearId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<CatechismClassDto>> GetClassesByYearAsync(Guid yearId, CancellationToken cancellationToken);
}