namespace TNTTBD.Services.Application.Contacts.Queries;

public record GetContactsQuery(int Page = 1, int? PageSize = null, string? Search = null);
