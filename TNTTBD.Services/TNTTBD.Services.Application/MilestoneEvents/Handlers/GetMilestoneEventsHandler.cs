using MediatR;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.MilestoneEvents.DTOs;
using TNTTBD.Services.Application.MilestoneEvents.Interfaces;
using TNTTBD.Services.Application.MilestoneEvents.Queries;

namespace TNTTBD.Services.Application.MilestoneEvents.Handlers;

public class GetMilestoneEventsHandler(
    IMilestoneEventQueryService queryService) : IRequestHandler<GetMilestoneEventsQuery, PaginatedResult<MilestoneEventSummaryDto>>
{
    public async Task<PaginatedResult<MilestoneEventSummaryDto>> Handle(GetMilestoneEventsQuery query, CancellationToken cancellationToken)
    {
        return await queryService.GetListAsync(query.Search, query.Type, query.From, query.To, Math.Max(1, query.Page), query.PageSize, cancellationToken);
    }
}
