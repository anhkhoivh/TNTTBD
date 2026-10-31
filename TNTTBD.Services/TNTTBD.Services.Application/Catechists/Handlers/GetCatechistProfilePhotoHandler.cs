using MediatR;
using TNTTBD.Services.Application.Catechists.Interfaces;
using TNTTBD.Services.Application.Catechists.Queries;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Application.Catechists.Handlers;

public class GetCatechistProfilePhotoHandler : IRequestHandler<GetCatechistProfilePhotoQuery, ProfilePhotoFileDto?>
{
    private readonly ICatechistQueryService _queryService;

    public GetCatechistProfilePhotoHandler(ICatechistQueryService queryService)
    {
        _queryService = queryService;
    }

    public Task<ProfilePhotoFileDto?> Handle(GetCatechistProfilePhotoQuery query, CancellationToken cancellationToken)
    {
        return _queryService.GetProfilePhotoAsync(query.CatechistId, cancellationToken);
    }
}
