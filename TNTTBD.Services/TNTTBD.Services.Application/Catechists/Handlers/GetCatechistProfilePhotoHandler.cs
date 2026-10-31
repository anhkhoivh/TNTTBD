using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Catechists.Interfaces;
using TNTTBD.Services.Application.Catechists.Queries;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.Common.Exceptions;

namespace TNTTBD.Services.Application.Catechists.Handlers;

public class GetCatechistProfilePhotoHandler : IRequestHandler<GetCatechistProfilePhotoQuery, ProfilePhotoFileDto?>
{
    private readonly ICatechistQueryService _queryService;
    private readonly IAuthorizationContext _authContext;

    public GetCatechistProfilePhotoHandler(
        ICatechistQueryService queryService,
        IAuthorizationContext authContext)
    {
        _queryService = queryService;
        _authContext = authContext;
    }

    public async Task<ProfilePhotoFileDto?> Handle(GetCatechistProfilePhotoQuery query, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanViewCatechistAsync(query.CatechistId, cancellationToken))
        {
            throw new ForbiddenException();
        }

        return await _queryService.GetProfilePhotoAsync(query.CatechistId, cancellationToken);
    }
}
