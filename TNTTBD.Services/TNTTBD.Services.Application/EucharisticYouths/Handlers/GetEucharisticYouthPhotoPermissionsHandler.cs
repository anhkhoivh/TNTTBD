using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.EucharisticYouths.Queries;

namespace TNTTBD.Services.Application.EucharisticYouths.Handlers;

public class GetEucharisticYouthPhotoPermissionsHandler : IRequestHandler<GetEucharisticYouthPhotoPermissionsQuery, bool>
{
    private readonly IAuthorizationContext _authContext;

    public GetEucharisticYouthPhotoPermissionsHandler(IAuthorizationContext authContext)
    {
        _authContext = authContext;
    }

    public Task<bool> Handle(GetEucharisticYouthPhotoPermissionsQuery query, CancellationToken cancellationToken)
    {
        return _authContext.CanManageYouthPhotosAsync(query.YouthId, cancellationToken);
    }
}
