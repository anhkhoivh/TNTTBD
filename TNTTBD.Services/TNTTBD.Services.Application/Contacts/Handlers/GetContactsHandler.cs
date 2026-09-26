using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.Contacts.DTOs;
using TNTTBD.Services.Application.Contacts.Interfaces;
using TNTTBD.Services.Application.Contacts.Queries;

namespace TNTTBD.Services.Application.Contacts.Handlers;

public class GetContactsHandler : IRequestHandler<GetContactsQuery, PaginatedResult<ContactDto>>
{
    private readonly IContactQueryService _queryService;
    private readonly IAuthorizationContext _authContext;

    public GetContactsHandler(IContactQueryService queryService, IAuthorizationContext authContext)
    {
        _queryService = queryService;
        _authContext = authContext;
    }

    public async Task<PaginatedResult<ContactDto>> Handle(GetContactsQuery query, CancellationToken cancellationToken)
    {
        if (!await _authContext.IsAdminOrCatechistAsync(cancellationToken))
        {
            return new PaginatedResult<ContactDto>
            {
                Items = new List<ContactDto>(),
                Total = 0,
                Page = query.Page,
                PageSize = query.PageSize ?? 0,
            };
        }

        return await _queryService.GetAllAsync(query.Page, query.PageSize, query.Search, cancellationToken);
    }
}
