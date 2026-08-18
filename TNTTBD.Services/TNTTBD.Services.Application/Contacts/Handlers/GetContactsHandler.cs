using MediatR;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.Contacts.DTOs;
using TNTTBD.Services.Application.Contacts.Interfaces;
using TNTTBD.Services.Application.Contacts.Queries;

namespace TNTTBD.Services.Application.Contacts.Handlers;

public class GetContactsHandler : IRequestHandler<GetContactsQuery, PaginatedResult<ContactDto>>
{
    private readonly IContactQueryService _queryService;

    public GetContactsHandler(IContactQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<PaginatedResult<ContactDto>> Handle(GetContactsQuery query, CancellationToken cancellationToken)
    {
        return await _queryService.GetAllAsync(query.Page, query.PageSize, query.Search, cancellationToken);
    }
}
