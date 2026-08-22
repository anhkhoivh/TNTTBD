using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.CatechismClasses.Commands;

public class RemoveCatechismClassHandler(ICatechismClassRepository repository, IAuthorizationContext authorizationContext) : IRequestHandler<RemoveCatechismClassCommand, bool>
{
    public async Task<bool> Handle(RemoveCatechismClassCommand command, CancellationToken cancellationToken)
    {
        if (!await authorizationContext.IsAdminAsync(cancellationToken))
        {
            return false;
        }
        await repository.Remove(command.CatechismClassId, cancellationToken);
        return true;
    }
}