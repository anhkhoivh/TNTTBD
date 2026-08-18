using MediatR;
using TNTTBD.Services.Application.CatechismSessions.DTOs;

namespace TNTTBD.Services.Application.CatechismSessions.Queries;

public record GetAttendanceSheetQuery(Guid SessionId) : IRequest<List<CatechismSessionAttendanceDto>>;
