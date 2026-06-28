using TNTTBD.Services.Application.CatechismClasses.DTOs;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Application.CatechismClasses.Interfaces;

public interface ICatechismClassQueryService
{
    Task<CatechismClassDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<CatechismClassDto>> GetClassesByYearAsync(Guid yearId, CancellationToken cancellationToken);
    Task<PaginatedResult<CatechismClassDto>> GetAllAsync(Guid? yearId, int page, int pageSize, CancellationToken cancellationToken);
}