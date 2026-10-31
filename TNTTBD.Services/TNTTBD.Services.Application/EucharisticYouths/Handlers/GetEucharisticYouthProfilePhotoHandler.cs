using MediatR;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.EucharisticYouths.Interfaces;
using TNTTBD.Services.Application.EucharisticYouths.Queries;

namespace TNTTBD.Services.Application.EucharisticYouths.Handlers;

public class GetEucharisticYouthProfilePhotoHandler : IRequestHandler<GetEucharisticYouthProfilePhotoQuery, ProfilePhotoFileDto?>
{
    private readonly IEucharisticYouthQueryService _queryService;

    public GetEucharisticYouthProfilePhotoHandler(IEucharisticYouthQueryService queryService)
    {
        _queryService = queryService;
    }

    public Task<ProfilePhotoFileDto?> Handle(GetEucharisticYouthProfilePhotoQuery query, CancellationToken cancellationToken)
    {
        return _queryService.GetProfilePhotoAsync(query.YouthId, cancellationToken);
    }
}
