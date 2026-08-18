using MediatR;
using TNTTBD.Services.Application.CatechismSessions.DTOs;
using TNTTBD.Services.Application.CatechismSessions.Interfaces;
using TNTTBD.Services.Application.CatechismSessions.Queries;

namespace TNTTBD.Services.Application.CatechismSessions.Handlers;

public class GetAttendanceSheetHandler : IRequestHandler<GetAttendanceSheetQuery, List<CatechismSessionAttendanceDto>>
{
    private readonly ICatechismSessionQueryService _queryService;

    public GetAttendanceSheetHandler(ICatechismSessionQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<List<CatechismSessionAttendanceDto>> Handle(GetAttendanceSheetQuery request, CancellationToken cancellationToken)
    {
        return await _queryService.GetAttendanceSheetAsync(request.SessionId, cancellationToken);
    }
}
