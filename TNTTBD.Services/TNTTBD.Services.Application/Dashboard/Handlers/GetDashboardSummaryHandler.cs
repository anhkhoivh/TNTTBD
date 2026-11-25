using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Dashboard.DTOs;
using TNTTBD.Services.Application.Dashboard.Interfaces;
using TNTTBD.Services.Application.Dashboard.Queries;

namespace TNTTBD.Services.Application.Dashboard.Handlers;

public class GetDashboardSummaryHandler : IRequestHandler<GetDashboardSummaryQuery, DashboardSummaryDto?>
{
    private readonly IDashboardQueryService _queryService;
    private readonly IAuthorizationContext _authContext;

    public GetDashboardSummaryHandler(IDashboardQueryService queryService, IAuthorizationContext authContext)
    {
        _queryService = queryService;
        _authContext = authContext;
    }

    public async Task<DashboardSummaryDto?> Handle(GetDashboardSummaryQuery query, CancellationToken cancellationToken)
    {
        Guid? userId = _authContext.GetCurrentUserId();
        return userId.HasValue
            ? await _queryService.GetSummaryAsync(userId.Value, cancellationToken)
            : null;
    }
}
