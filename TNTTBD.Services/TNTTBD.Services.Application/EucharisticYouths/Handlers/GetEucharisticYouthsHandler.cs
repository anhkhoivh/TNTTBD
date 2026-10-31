using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.Common.Exceptions;
using TNTTBD.Services.Application.EucharisticYouths.DTOs;
using TNTTBD.Services.Application.EucharisticYouths.Interfaces;
using TNTTBD.Services.Application.EucharisticYouths.Queries;

namespace TNTTBD.Services.Application.EucharisticYouths.Handlers;

public class GetEucharisticYouthsHandler : IRequestHandler<GetEucharisticYouthsQuery, PaginatedResult<EucharisticYouthDto>>
{
    private readonly IEucharisticYouthQueryService _queryService;
    private readonly IFileStorage _fileStorage;
    private readonly IAuthorizationContext _authContext;

    public GetEucharisticYouthsHandler(
        IEucharisticYouthQueryService queryService,
        IFileStorage fileStorage,
        IAuthorizationContext authContext)
    {
        _queryService = queryService;
        _fileStorage = fileStorage;
        _authContext = authContext;
    }

    public async Task<PaginatedResult<EucharisticYouthDto>> Handle(GetEucharisticYouthsQuery query, CancellationToken cancellationToken)
    {
        if (!await _authContext.IsAdminOrCatechistAsync(cancellationToken))
        {
            throw new ForbiddenException();
        }

        return await _queryService.GetAllAsync(query.Search, query.ClassId, query.Page, query.PageSize, _fileStorage, cancellationToken);
    }
}
