using TNTTBD.Services.Application.CatechismSessions.DTOs;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Application.CatechismSessions.Interfaces;

public interface ICatechismSessionQueryService
{
    Task<CatechismSessionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<CatechismSessionDto>?> GetSessionsByClassAsync(Guid classId, CancellationToken cancellationToken);
    Task<PaginatedResult<CatechismSessionDto>> GetAllAsync(Guid? classId, int page, int? pageSize, CancellationToken cancellationToken);
    Task<PaginatedResult<CatechismSessionAttendanceDto>> GetAttendancesBySessionAsync(Guid sessionId, int page, int? pageSize, CancellationToken cancellationToken);
    Task<List<CatechismSessionAttendanceDto>> GetAttendanceSheetAsync(Guid sessionId, CancellationToken cancellationToken);
}