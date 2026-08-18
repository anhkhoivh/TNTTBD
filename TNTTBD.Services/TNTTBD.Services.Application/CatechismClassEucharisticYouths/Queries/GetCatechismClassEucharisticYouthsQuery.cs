using MediatR;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.DTOs;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Application.CatechismClassEucharisticYouths.Queries;

public record GetCatechismClassEucharisticYouthsQuery(Guid? ClassId = null, int Page = 1, int? PageSize = null) : IRequest<PaginatedResult<CatechismClassEucharisticYouthDto>>;
