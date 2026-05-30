using MediatR;
using TNTTBD.Services.Application.CatechismSessions.DTOs;
using TNTTBD.Services.Application.CatechismSessions.Interfaces;
using TNTTBD.Services.Application.CatechismSessions.Queries;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Application.CatechismSessions.Handlers;

public class GetCatechismSessionsHandler : IRequestHandler<GetCatechismSessionsQuery, PaginatedResult<CatechismSessionDto>>
{
    private readonly ICatechismSessionQueryService _queryService;

    public GetCatechismSessionsHandler(ICatechismSessionQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<PaginatedResult<CatechismSessionDto>> Handle(GetCatechismSessionsQuery query, CancellationToken cancellationToken)
    {
        return await _queryService.GetAllAsync(query.ClassId, query.Page, query.PageSize, cancellationToken);
    }
}
