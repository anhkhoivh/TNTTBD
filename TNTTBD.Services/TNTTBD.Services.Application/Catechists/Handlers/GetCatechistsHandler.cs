using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Catechists.DTOs;
using TNTTBD.Services.Application.Catechists.Interfaces;
using TNTTBD.Services.Application.Catechists.Queries;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Application.Catechists.Handlers;

public class GetCatechistsHandler : IRequestHandler<GetCatechistsQuery, PaginatedResult<CatechistDto>>
{
    private readonly ICatechistQueryService _queryService;
    private readonly IFileStorage _fileStorage;
    private readonly IAuthorizationContext _authContext;

    public GetCatechistsHandler(ICatechistQueryService queryService, IFileStorage fileStorage, IAuthorizationContext authContext)
    {
        _queryService = queryService;
        _fileStorage = fileStorage;
        _authContext = authContext;
    }

    public async Task<PaginatedResult<CatechistDto>> Handle(GetCatechistsQuery query, CancellationToken cancellationToken)
    {
        Guid? currentCatechistId = await _authContext.GetCurrentCatechistIdAsync(cancellationToken);
        return await _queryService.GetAllAsync(query.Page, query.PageSize, query.Search, currentCatechistId, _fileStorage, cancellationToken);
    }
}
