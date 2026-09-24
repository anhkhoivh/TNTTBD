using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Contacts.Commands;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.Contacts.Handlers;

public class DeleteContactHandler : IRequestHandler<DeleteContactCommand, bool>
{
    private readonly IAuthorizationContext _authContext;
    private readonly IContactRepository _contactRepository;

    public DeleteContactHandler(IAuthorizationContext authContext, IContactRepository contactRepository)
    {
        _authContext = authContext;
        _contactRepository = contactRepository;
    }

    public async Task<bool> Handle(DeleteContactCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.IsAdminOrCatechistAsync(cancellationToken))
        {
            return false;
        }

        await _contactRepository.RemoveAsync(command.Id, cancellationToken);

        return true;
    }
}
