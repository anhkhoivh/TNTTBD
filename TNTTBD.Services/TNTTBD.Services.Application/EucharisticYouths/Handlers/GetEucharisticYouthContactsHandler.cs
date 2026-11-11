using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.EucharisticYouths.DTOs;
using TNTTBD.Services.Application.EucharisticYouths.Interfaces;
using TNTTBD.Services.Application.EucharisticYouths.Queries;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.EucharisticYouths.Handlers;

public class GetEucharisticYouthContactsHandler : IRequestHandler<GetEucharisticYouthContactsQuery, List<EucharisticYouthContactDto>>
{
    private readonly IEucharisticYouthContactQueryService _queryService;
    private readonly IAuthorizationContext _authContext;

    public GetEucharisticYouthContactsHandler(IEucharisticYouthContactQueryService queryService, IAuthorizationContext authContext)
    {
        _queryService = queryService;
        _authContext = authContext;
    }

    public async Task<List<EucharisticYouthContactDto>> Handle(GetEucharisticYouthContactsQuery query, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanReadYouthContactsAsync(query.EucharisticYouthId, cancellationToken))
        {
            return new List<EucharisticYouthContactDto>();
        }

        List<EucharisticYouthContactDto> contacts = await _queryService.GetByYouthIdAsync(query.EucharisticYouthId, cancellationToken);

        // Parents / youths get a limited view: no godparent phone number or email
        if (!await _authContext.IsAdminOrCatechistAsync(cancellationToken))
        {
            foreach (EucharisticYouthContactDto c in contacts.Where(c => c.ContactRelationType == ContactRelationType.Godparent))
            {
                c.Contact.PhoneNumber = null;
                c.Contact.Email = null;
            }
        }

        return contacts;
    }
}
