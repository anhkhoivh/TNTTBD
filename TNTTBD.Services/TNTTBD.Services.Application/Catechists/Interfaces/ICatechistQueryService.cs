using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Catechists.DTOs;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Application.Catechists.Interfaces;

public interface ICatechistQueryService
{
    /// <summary>
    /// Get catechist by id
    /// </summary>
    Task<CatechistDto?> GetByIdAsync(Guid id, IFileStorage storage, CancellationToken cancellationToken);

    /// <summary>
    /// Get photo list
    /// </summary>
    /// <param name="id"></param>
    /// <param name="storage"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<PhotoReferenceDto>?> GetPhotosByCatechistId(Guid catechistId, IFileStorage storage, CancellationToken cancellationToken);
    Task<PaginatedResult<CatechistDto>> GetAllAsync(int page, int? pageSize, IFileStorage storage, CancellationToken cancellationToken);

    /// <summary>
    /// Resolve the catechist's active class in the most recent catechism year.
    /// </summary>
    Task<CatechistCurrentClassDto?> GetCurrentClassAsync(Guid catechistId, CancellationToken cancellationToken);
}