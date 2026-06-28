using MediatR;
using TNTTBD.Services.Application.CatechismSessions.DTOs;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Application.CatechismSessions.Queries;

public record GetAttendancesBySessionQuery(Guid SessionId, int Page = 1, int PageSize = 50) : IRequest<PaginatedResult<CatechismSessionAttendanceDto>>;
