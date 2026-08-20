using TNTTBD.Services.Application.Dashboard.DTOs;

namespace TNTTBD.Services.Application.Dashboard.Interfaces;

public interface IDashboardQueryService
{
    Task<DashboardSummaryDto> GetSummaryAsync(Guid userId, CancellationToken cancellationToken);
}
