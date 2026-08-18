using TNTTBD.Services.Application.CatechismYears.DTOs;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Application.CatechismYears.Interfaces;

public interface ICatechismYearQueryService
{
    Task<CatechismYearDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PaginatedResult<CatechismYearDto>> GetAllAsync(int page, int? pageSize, CancellationToken cancellationToken);
}