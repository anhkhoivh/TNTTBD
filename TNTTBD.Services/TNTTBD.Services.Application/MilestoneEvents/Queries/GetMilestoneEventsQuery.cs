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
