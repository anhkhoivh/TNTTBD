namespace TNTTBD.Services.Application.Catechists.Queries;

public record GetCatechistsQuery(int Page = 1, int? PageSize = null, string? Search = null);
