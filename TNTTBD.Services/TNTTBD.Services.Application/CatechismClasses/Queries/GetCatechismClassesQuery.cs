namespace TNTTBD.Services.Application.CatechismClasses.Queries;

public record GetCatechismClassesQuery(Guid? YearId = null, int Page = 1, int? PageSize = null);
