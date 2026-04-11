using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.EucharisticYouths.DTOs;

namespace TNTTBD.Services.Application.EucharisticYouths.Interfaces;

public interface IEucharisticYouthQueryService
{
    /// <summary>
    /// Get Eucharistic Youth by id
    /// </summary>
    Task<EucharisticYouthDto?> GetByIdAsync(Guid id, IFileStorage storage, CancellationToken cancellationToken);
}