using MediatR;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.Contacts.DTOs;

namespace TNTTBD.Services.Application.Contacts.Queries;

public record GetContactsQuery(int Page = 1, int? PageSize = null, string? Search = null) : IRequest<PaginatedResult<ContactDto>>;
