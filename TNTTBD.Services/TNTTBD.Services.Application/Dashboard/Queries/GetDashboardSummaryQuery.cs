using MediatR;
using TNTTBD.Services.Application.Dashboard.DTOs;

namespace TNTTBD.Services.Application.Dashboard.Queries;

public record GetDashboardSummaryQuery : IRequest<DashboardSummaryDto?>;
