using TNTTBD.Services.Application.CatechismClassEucharisticYouths.DTOs;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Application.CatechismClassEucharisticYouths.Interfaces;

public interface ICatechismClassEucharisticYouthQueryService
{
    /// <summary>
    /// Get class by id
    /// </summary>
    Task<CatechismClassEucharisticYouthDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Get youth list of class
    /// </summary>
    /// <param name="classId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<CatechismClassEucharisticYouthDto>> GetCatechismClassEucharisticYouthsByClassAsync(Guid classId, CancellationToken cancellationToken);

    /// <summary>
    /// Get class list of youth
    /// </summary>
    /// <param name="youthId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<CatechismClassEucharisticYouthDto>> GetCatechismClassEucharisticYouthsByYouthAsync(Guid youthId, CancellationToken cancellationToken);
    Task<PaginatedResult<CatechismClassEucharisticYouthDto>> GetAllAsync(Guid? classId, int page, int? pageSize, CancellationToken cancellationToken);
}