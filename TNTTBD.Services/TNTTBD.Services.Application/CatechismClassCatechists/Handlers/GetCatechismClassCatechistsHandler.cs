using MediatR;
using TNTTBD.Services.Application.CatechismClassCatechists.DTOs;
using TNTTBD.Services.Application.CatechismClassCatechists.Interfaces;
using TNTTBD.Services.Application.CatechismClassCatechists.Queries;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Application.CatechismClassCatechists.Handlers;

public class GetCatechismClassCatechistsHandler : IRequestHandler<GetCatechismClassCatechistsQuery, PaginatedResult<CatechismClassCatechistDto>>
{
    private readonly ICatechismClassCatechistQueryService _queryService;

    public GetCatechismClassCatechistsHandler(ICatechismClassCatechistQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<PaginatedResult<CatechismClassCatechistDto>> Handle(GetCatechismClassCatechistsQuery query, CancellationToken cancellationToken)
    {
        return await _queryService.GetAllAsync(query.ClassId, query.CatechistId, query.Page, query.PageSize, cancellationToken);
    }
}
