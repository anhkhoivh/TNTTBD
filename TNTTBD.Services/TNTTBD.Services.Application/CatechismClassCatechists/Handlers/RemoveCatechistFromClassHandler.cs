using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.CatechismClassCatechists.Commands;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.CatechismClassCatechists.Handlers;

public class RemoveCatechistFromClassHandler(ICatechismClassCatechistRepository repository, IAuthorizationContext authorizationContext) : IRequestHandler<RemoveCatechistFromClassCommand, bool>
{
    public async Task<bool> Handle(RemoveCatechistFromClassCommand command, CancellationToken cancellationToken)
    {
        if (!await authorizationContext.IsAdminAsync(cancellationToken))
        {
            return false;
        }
        await repository.Remove(command.Id, cancellationToken);
        return true;
    }
}
