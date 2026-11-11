using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.MilestoneEvents.DTOs;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.MilestoneEvents.Interfaces;

public interface IMilestoneEventQueryService
{
    Task<PaginatedResult<MilestoneEventSummaryDto>> GetListAsync(string? search, MilestoneType? type, DateOnly? from, DateOnly? to, int page, int? pageSize, CancellationToken cancellationToken);

    Task<MilestoneEventDetailDto?> GetDetailAsync(Guid id, CancellationToken cancellationToken);

    Task<PaginatedResult<MilestoneEventCandidateDto>> GetCandidatesAsync(MilestoneType type, DateOnly? date, string? search, Guid? classId, Guid? catechismYearId, int page, int? pageSize, CancellationToken cancellationToken);

    Task<MilestoneEventExportDto?> GetExportDataAsync(Guid id, CancellationToken cancellationToken);
}

public interface IMilestoneEventExcelGenerator
{
    byte[] Generate(MilestoneEventExportDto data, string language);
}
