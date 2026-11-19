using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Contacts.Commands;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Common;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.Contacts.Handlers;

public class UpdateContactHandler : IRequestHandler<UpdateContactCommand, bool>
{
    private readonly IAuthorizationContext _authContext;
    private readonly IContactRepository _contactRepository;

    public UpdateContactHandler(IAuthorizationContext authContext, IContactRepository contactRepository)
    {
        _authContext = authContext;
        _contactRepository = contactRepository;
    }

    public async Task<bool> Handle(UpdateContactCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanAccessContactAsync(command.Id, cancellationToken))
        {
            return false;
        }

        Contact contact = await _contactRepository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new DomainException(DomainErrors.ValidationNotFound);

        FullName fullName = new FullName(command.GivenName, command.ChristName, command.FamilyName, command.MiddleName);
        contact.UpdateDetails(fullName, command.PhoneNumber, command.Email);
        await _contactRepository.UpdateAsync(contact, cancellationToken);

        return true;
    }
}
