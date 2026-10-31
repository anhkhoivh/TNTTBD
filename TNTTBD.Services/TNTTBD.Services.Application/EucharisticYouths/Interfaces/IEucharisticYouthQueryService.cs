using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.EucharisticYouths.DTOs;

namespace TNTTBD.Services.Application.EucharisticYouths.Interfaces;

public interface IEucharisticYouthQueryService
{
    /// <summary>
    /// Get Eucharistic Youth by id
    /// </summary>
    Task<EucharisticYouthDto?> GetByIdAsync(Guid id, IFileStorage storage, CancellationToken cancellationToken);

    /// <summary>
    /// Get photos
    /// </summary>
    /// <param name="youthId"></param>
    /// <param name="storage"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<PhotoReferenceDto>?> GetPhotosByYouthId(Guid youthId, IFileStorage storage, CancellationToken cancellationToken);

    /// <summary>
    /// Get the current (newest) profile photo, or null when there is none.
    /// </summary>
    Task<ProfilePhotoFileDto?> GetProfilePhotoAsync(Guid youthId, CancellationToken cancellationToken);
    Task<PaginatedResult<EucharisticYouthDto>> GetAllAsync(string? search, Guid? classId, int page, int? pageSize, IFileStorage storage, CancellationToken cancellationToken);
}