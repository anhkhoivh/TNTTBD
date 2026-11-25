using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Catechists.Queries;

namespace TNTTBD.Services.Application.Catechists.Handlers;

public class GetCatechistPhotoPermissionsHandler : IRequestHandler<GetCatechistPhotoPermissionsQuery, bool>
{
    private readonly IAuthorizationContext _authContext;

    public GetCatechistPhotoPermissionsHandler(IAuthorizationContext authContext)
    {
        _authContext = authContext;
    }

    public Task<bool> Handle(GetCatechistPhotoPermissionsQuery query, CancellationToken cancellationToken)
    {
        return _authContext.CanManageCatechistPhotosAsync(query.CatechistId, cancellationToken);
    }
}
