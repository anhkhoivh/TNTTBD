using MediatR;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.DTOs;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.Interfaces;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.Queries;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Application.CatechismClassEucharisticYouths.Handlers;

public class GetCatechismClassEucharisticYouthsHandler : IRequestHandler<GetCatechismClassEucharisticYouthsQuery, PaginatedResult<CatechismClassEucharisticYouthDto>>
{
    private readonly ICatechismClassEucharisticYouthQueryService _queryService;

    public GetCatechismClassEucharisticYouthsHandler(ICatechismClassEucharisticYouthQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<PaginatedResult<CatechismClassEucharisticYouthDto>> Handle(GetCatechismClassEucharisticYouthsQuery query, CancellationToken cancellationToken)
    {
        return await _queryService.GetAllAsync(query.ClassId, query.Page, query.PageSize, cancellationToken);
    }
}
