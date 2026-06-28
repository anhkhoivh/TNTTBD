using MediatR;
using TNTTBD.Services.Application.CatechismYears.DTOs;
using TNTTBD.Services.Application.CatechismYears.Interfaces;
using TNTTBD.Services.Application.CatechismYears.Queries;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Application.CatechismYears.Handlers;

public class GetCatechismYearsHandler : IRequestHandler<GetCatechismYearsQuery, PaginatedResult<CatechismYearDto>>
{
    private readonly ICatechismYearQueryService _queryService;

    public GetCatechismYearsHandler(ICatechismYearQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<PaginatedResult<CatechismYearDto>> Handle(GetCatechismYearsQuery query, CancellationToken cancellationToken)
    {
        return await _queryService.GetAllAsync(query.Page, query.PageSize, cancellationToken);
    }
}
