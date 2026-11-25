using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.DTOs;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.Interfaces;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.Queries;

namespace TNTTBD.Services.Application.CatechismClassEucharisticYouths.Handlers;

public class GetCatechismClassEucharisticYouthsByYouthHandler : IRequestHandler<GetCatechismClassEucharisticYouthsByYouthQuery, List<CatechismClassEucharisticYouthDto>>
{
    private readonly ICatechismClassEucharisticYouthQueryService _queryService;

    public GetCatechismClassEucharisticYouthsByYouthHandler(ICatechismClassEucharisticYouthQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<List<CatechismClassEucharisticYouthDto>> Handle(GetCatechismClassEucharisticYouthsByYouthQuery query, CancellationToken cancellationToken)
    {
        return await _queryService.GetCatechismClassEucharisticYouthsByYouthAsync(query.EucharisticYouthId, cancellationToken);
    }
}
