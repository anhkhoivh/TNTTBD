using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Catechists.DTOs;
using TNTTBD.Services.Application.Catechists.Interfaces;
using TNTTBD.Services.Application.Catechists.Queries;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Application.Catechists.Handlers;

public class GetCatechistsHandler : IRequestHandler<GetCatechistsQuery, PaginatedResult<CatechistDto>>
{
    private readonly ICatechistQueryService _queryService;
    private readonly IFileStorage _fileStorage;

    public GetCatechistsHandler(ICatechistQueryService queryService, IFileStorage fileStorage)
    {
        _queryService = queryService;
        _fileStorage = fileStorage;
    }

    public async Task<PaginatedResult<CatechistDto>> Handle(GetCatechistsQuery query, CancellationToken cancellationToken)
    {
        return await _queryService.GetAllAsync(query.Page, query.PageSize, _fileStorage, cancellationToken);
    }
}
