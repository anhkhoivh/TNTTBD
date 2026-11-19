using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Contacts.Commands;
using TNTTBD.Services.Domain.Catechism.JoinModels;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Common;

namespace TNTTBD.Services.Application.Contacts.Handlers;

public class LinkContactToYouthHandler : IRequestHandler<LinkContactToYouthCommand, Guid?>
{
    private readonly IAuthorizationContext _authContext;
    private readonly IContactRepository _contactRepository;
    private readonly IEucharisticYouthContactRepository _youthContactRepository;

    public LinkContactToYouthHandler(
        IAuthorizationContext authContext,
        IContactRepository contactRepository,
        IEucharisticYouthContactRepository youthContactRepository)
    {
        _authContext = authContext;
        _contactRepository = contactRepository;
        _youthContactRepository = youthContactRepository;
    }

    public async Task<Guid?> Handle(LinkContactToYouthCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanAccessYouthAsync(command.EucharisticYouthId, cancellationToken))
        {
            return null;
        }

        if (await _contactRepository.GetByIdAsync(command.ContactId, cancellationToken) == null)
        {
            throw new DomainException(DomainErrors.ValidationNotFound);
        }

        if (await _youthContactRepository.ExistsAsync(command.EucharisticYouthId, command.ContactId, cancellationToken))
        {
            throw new DomainException(DomainErrors.ContactAlreadyLinked);
        }

        EucharisticYouthContact link = new EucharisticYouthContact(
            Guid.NewGuid(),
            command.EucharisticYouthId,
            command.ContactId,
            command.RelationType);
        await _youthContactRepository.AddAsync(link, cancellationToken);

        return link.Id;
    }
}
