namespace TNTTBD.Services.Application.EucharisticYouths.Queries;

public record GetEucharisticYouthsQuery(string? Search = null, Guid? ClassId = null, int Page = 1, int? PageSize = null);
