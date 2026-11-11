using MediatR;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.MilestoneEvents.DTOs;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.MilestoneEvents.Queries;

public record GetMilestoneEventsQuery(
    string? Search,
    MilestoneType? Type,
    DateOnly? From,
    DateOnly? To,
    int Page = 1,
    int? PageSize = null
) : IRequest<PaginatedResult<MilestoneEventSummaryDto>>;

public record GetMilestoneEventQuery(Guid Id) : IRequest<MilestoneEventDetailDto?>;

public record GetMilestoneEventCandidatesQuery(
    MilestoneType Type,
    string? Search,
    Guid? ClassId,
    Guid? CatechismYearId,
    DateOnly? Date = null,
    int Page = 1,
    int? PageSize = 20
) : IRequest<PaginatedResult<MilestoneEventCandidateDto>>;

public record GenerateMilestoneCodesQuery(
    string? Template,
    int StartIndex,
    List<DateOnly> Dates
) : IRequest<GenerateMilestoneCodesResultDto>;

public record ExportMilestoneEventExcelQuery(Guid Id, string Language) : IRequest<MilestoneEventExcelFileDto?>;
