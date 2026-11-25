namespace TNTTBD.Services.Application.CatechismSessions.Queries;

public record GetCatechismSessionsQuery(Guid? ClassId = null, int Page = 1, int? PageSize = null);
