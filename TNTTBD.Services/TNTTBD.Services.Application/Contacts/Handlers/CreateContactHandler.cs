using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Contacts.Commands;
using TNTTBD.Services.Domain.Catechism.JoinModels;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Enums;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.Contacts.Handlers;

public class CreateContactHandler : IRequestHandler<CreateContactCommand, Guid?>
{
    private readonly IAuthorizationContext _authContext;
    private readonly IContactRepository _contactRepository;
    private readonly IEucharisticYouthContactRepository _youthContactRepository;

    public CreateContactHandler(
        IAuthorizationContext authContext,
        IContactRepository contactRepository,
        IEucharisticYouthContactRepository youthContactRepository)
    {
        _authContext = authContext;
        _contactRepository = contactRepository;
        _youthContactRepository = youthContactRepository;
    }

    public async Task<Guid?> Handle(CreateContactCommand command, CancellationToken cancellationToken)
    {
        bool allowed = command.LinkEucharisticYouthId.HasValue
            ? await _authContext.CanAccessYouthAsync(command.LinkEucharisticYouthId.Value, cancellationToken)
            : await _authContext.IsAdminOrCatechistAsync(cancellationToken);
        if (!allowed)
        {
            return null;
        }

        FullName fullName = new FullName(command.GivenName, command.ChristName, command.FamilyName, command.MiddleName);
        Contact contact = new Contact(Guid.NewGuid(), fullName, command.PhoneNumber, command.Email);
        await _contactRepository.AddAsync(contact, cancellationToken);

        if (command.LinkEucharisticYouthId.HasValue)
        {
            EucharisticYouthContact link = new EucharisticYouthContact(
                Guid.NewGuid(),
                command.LinkEucharisticYouthId.Value,
                contact.Id,
                command.LinkRelationType ?? ContactRelationType.Undefined);
            await _youthContactRepository.AddAsync(link, cancellationToken);
        }

        return contact.Id;
    }
}
