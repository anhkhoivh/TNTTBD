using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.EucharisticYouths.DTOs;
using TNTTBD.Services.Application.EucharisticYouths.Interfaces;
using TNTTBD.Services.Application.EucharisticYouths.Queries;

namespace TNTTBD.Services.Application.EucharisticYouths.Handlers;

public class GetEucharisticYouthContactsHandler : IRequestHandler<GetEucharisticYouthContactsQuery, List<EucharisticYouthContactDto>>
{
    private readonly IEucharisticYouthContactQueryService _queryService;
    private readonly IAuthorizationContext _authContext;

    public GetEucharisticYouthContactsHandler(IEucharisticYouthContactQueryService queryService, IAuthorizationContext authContext)
    {
        _queryService = queryService;
        _authContext = authContext;
    }

    public async Task<List<EucharisticYouthContactDto>> Handle(GetEucharisticYouthContactsQuery query, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanReadYouthContactsAsync(query.EucharisticYouthId, cancellationToken))
        {
            return new List<EucharisticYouthContactDto>();
        }

        return await _queryService.GetByYouthIdAsync(query.EucharisticYouthId, cancellationToken);
    }
}
