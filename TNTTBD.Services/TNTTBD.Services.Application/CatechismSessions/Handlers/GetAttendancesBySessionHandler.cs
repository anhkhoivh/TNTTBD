using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.CatechismSessions.DTOs;
using TNTTBD.Services.Application.CatechismSessions.Interfaces;
using TNTTBD.Services.Application.CatechismSessions.Queries;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Application.CatechismSessions.Handlers;

public class GetAttendancesBySessionHandler : IRequestHandler<GetAttendancesBySessionQuery, PaginatedResult<CatechismSessionAttendanceDto>>
{
    private readonly ICatechismSessionQueryService _queryService;

    public GetAttendancesBySessionHandler(ICatechismSessionQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<PaginatedResult<CatechismSessionAttendanceDto>> Handle(GetAttendancesBySessionQuery request, CancellationToken cancellationToken)
    {
        return await _queryService.GetAttendancesBySessionAsync(request.SessionId, request.Page, request.PageSize, cancellationToken);
    }
}
