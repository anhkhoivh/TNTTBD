using TNTTBD.Services.Application.CatechismYears.DTOs;

namespace TNTTBD.Services.Application.CatechismYears.Interfaces;

public interface ICatechismYearQueryService
{
    /// <summary>
    /// Get year by id
    /// </summary>
    Task<CatechismYearDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}