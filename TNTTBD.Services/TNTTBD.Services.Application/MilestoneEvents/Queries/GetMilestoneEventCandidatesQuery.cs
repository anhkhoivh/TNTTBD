using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.MilestoneEvents.Queries;

public record GetMilestoneEventCandidatesQuery(
    MilestoneType Type,
    string? Search,
    Guid? ClassId,
    Guid? CatechismYearId,
    DateOnly? Date = null,
    int Page = 1,
    int? PageSize = 20
);
