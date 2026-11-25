using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.CatechismClasses.DTOs;
using TNTTBD.Services.Application.CatechismClasses.Interfaces;
using TNTTBD.Services.Application.CatechismClasses.Queries;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Application.CatechismClasses.Handlers;

public class GetCatechismClassesHandler : IRequestHandler<GetCatechismClassesQuery, PaginatedResult<CatechismClassDto>>
{
    private readonly ICatechismClassQueryService _queryService;

    public GetCatechismClassesHandler(ICatechismClassQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<PaginatedResult<CatechismClassDto>> Handle(GetCatechismClassesQuery query, CancellationToken cancellationToken)
    {
        return await _queryService.GetAllAsync(query.YearId, query.Page, query.PageSize, cancellationToken);
    }
}
