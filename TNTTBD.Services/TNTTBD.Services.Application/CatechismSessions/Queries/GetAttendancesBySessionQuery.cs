namespace TNTTBD.Services.Application.CatechismSessions.Queries;

public record GetAttendancesBySessionQuery(Guid SessionId, int Page = 1, int? PageSize = null);
