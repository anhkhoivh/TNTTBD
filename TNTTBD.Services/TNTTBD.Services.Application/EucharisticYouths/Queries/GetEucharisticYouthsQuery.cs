using MediatR;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.EucharisticYouths.DTOs;

namespace TNTTBD.Services.Application.EucharisticYouths.Queries;

public record GetEucharisticYouthsQuery(string? Search = null, int Page = 1, int? PageSize = null) : IRequest<PaginatedResult<EucharisticYouthDto>>;
