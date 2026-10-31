using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.Common.Exceptions;
using TNTTBD.Services.Application.EucharisticYouths.Interfaces;
using TNTTBD.Services.Application.EucharisticYouths.Queries;

namespace TNTTBD.Services.Application.EucharisticYouths.Handlers;

public class GetEucharisticYouthProfilePhotoHandler : IRequestHandler<GetEucharisticYouthProfilePhotoQuery, ProfilePhotoFileDto?>
{
    private readonly IEucharisticYouthQueryService _queryService;
    private readonly IAuthorizationContext _authContext;

    public GetEucharisticYouthProfilePhotoHandler(
        IEucharisticYouthQueryService queryService,
        IAuthorizationContext authContext)
    {
        _queryService = queryService;
        _authContext = authContext;
    }

    public async Task<ProfilePhotoFileDto?> Handle(GetEucharisticYouthProfilePhotoQuery query, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanViewYouthAsync(query.YouthId, cancellationToken))
        {
            throw new ForbiddenException();
        }

        return await _queryService.GetProfilePhotoAsync(query.YouthId, cancellationToken);
    }
}
