using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.CatechismClassCatechists.Commands;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.CatechismClassCatechists.Handlers;

public class UpdateCatechistClassAssignmentHandler(ICatechismClassCatechistRepository repository, IAuthorizationContext authContext) : IRequestHandler<UpdateCatechistClassAssignmentCommand, bool>
{
    public async Task<bool> Handle(UpdateCatechistClassAssignmentCommand command, CancellationToken cancellationToken)
    {
        if (await authContext.IsAdminAsync(cancellationToken))
        {
            return false;
        }
        await repository.Update(command.Id, command.Role, command.Note, command.LeftAt, cancellationToken);
        return true;
    }
}
