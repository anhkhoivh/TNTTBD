using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Catechists.DTOs;

namespace TNTTBD.Services.Application.Catechists.Interfaces;

public interface ICatechistQueryService
{
    /// <summary>
    /// Get catechist by id
    /// </summary>
    Task<CatechistDto?> GetByIdAsync(Guid id, IFileStorage storage, CancellationToken cancellationToken);
}