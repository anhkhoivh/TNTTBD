using TNTTBD.Services.Application.CatechismClassCatechists.DTOs;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Application.CatechismClassCatechists.Interfaces;

public interface ICatechismClassCatechistQueryService
{
    Task<CatechismClassCatechistDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<CatechismClassCatechistDto>> GetByClassAsync(Guid classId, CancellationToken cancellationToken);
    Task<List<CatechismClassCatechistDto>> GetByCatechistAsync(Guid catechistId, CancellationToken cancellationToken);
    Task<PaginatedResult<CatechismClassCatechistDto>> GetAllAsync(Guid? classId, int page, int? pageSize, CancellationToken cancellationToken);
}
