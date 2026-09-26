using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Contacts.DTOs;
using TNTTBD.Services.Application.Contacts.Interfaces;
using TNTTBD.Services.Application.Contacts.Queries;

namespace TNTTBD.Services.Application.Contacts.Handlers;

public class GetContactByIdHandler : IRequestHandler<GetContactByIdQuery, ContactDetailDto?>
{
    private readonly IAuthorizationContext _authContext;
    private readonly IContactQueryService _queryService;

    public GetContactByIdHandler(IAuthorizationContext authContext, IContactQueryService queryService)
    {
        _authContext = authContext;
        _queryService = queryService;
    }

    public async Task<ContactDetailDto?> Handle(GetContactByIdQuery query, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanAccessContactAsync(query.Id, cancellationToken))
        {
            return null;
        }

        ContactDetailDto? dto = await _queryService.GetByIdAsync(query.Id, cancellationToken);
        if (dto == null)
        {
            return null;
        }

        // Youth-role users only see the youths they can access (themselves)
        bool seesAll = await _authContext.IsAdminOrCatechistAsync(cancellationToken)
            || await _authContext.GetCurrentContactIdAsync(cancellationToken) == query.Id;
        if (!seesAll)
        {
            List<ContactYouthDto> visible = new();
            foreach (ContactYouthDto youth in dto.Youths)
            {
                if (await _authContext.CanAccessYouthAsync(youth.EucharisticYouthId, cancellationToken))
                {
                    visible.Add(youth);
                }
            }
            dto.Youths = visible;
        }

        return dto;
    }
}
