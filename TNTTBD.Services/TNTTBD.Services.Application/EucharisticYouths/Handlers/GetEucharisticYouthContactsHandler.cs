using MediatR;
using TNTTBD.Services.Application.EucharisticYouths.DTOs;
using TNTTBD.Services.Application.EucharisticYouths.Interfaces;
using TNTTBD.Services.Application.EucharisticYouths.Queries;

namespace TNTTBD.Services.Application.EucharisticYouths.Handlers;

public class GetEucharisticYouthContactsHandler : IRequestHandler<GetEucharisticYouthContactsQuery, List<EucharisticYouthContactDto>>
{
    private readonly IEucharisticYouthContactQueryService _queryService;

    public GetEucharisticYouthContactsHandler(IEucharisticYouthContactQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<List<EucharisticYouthContactDto>> Handle(GetEucharisticYouthContactsQuery query, CancellationToken cancellationToken)
    {
        return await _queryService.GetByYouthIdAsync(query.EucharisticYouthId, cancellationToken);
    }
}
