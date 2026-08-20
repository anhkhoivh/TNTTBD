using MediatR;
using TNTTBD.Services.Application.Dashboard.DTOs;
using TNTTBD.Services.Application.Dashboard.Interfaces;
using TNTTBD.Services.Application.Dashboard.Queries;

namespace TNTTBD.Services.Application.Dashboard.Handlers;

public class GetDashboardSummaryHandler : IRequestHandler<GetDashboardSummaryQuery, DashboardSummaryDto>
{
    private readonly IDashboardQueryService _queryService;

    public GetDashboardSummaryHandler(IDashboardQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<DashboardSummaryDto> Handle(GetDashboardSummaryQuery query, CancellationToken cancellationToken)
    {
        return await _queryService.GetSummaryAsync(query.UserId, cancellationToken);
    }
}
