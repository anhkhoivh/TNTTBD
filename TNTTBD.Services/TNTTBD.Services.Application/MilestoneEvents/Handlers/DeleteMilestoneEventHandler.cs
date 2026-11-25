using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Common.Exceptions;
using TNTTBD.Services.Application.MilestoneEvents.Commands;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.MilestoneEvents.Handlers;

public class DeleteMilestoneEventHandler(
    IMilestoneEventRepository repository,
    IAuthorizationContext auth) : IRequestHandler<DeleteMilestoneEventCommand, bool>
{
    public async Task<bool> Handle(DeleteMilestoneEventCommand command, CancellationToken cancellationToken)
    {
        if (!await auth.IsAdminAsync(cancellationToken))
        {
            throw new ForbiddenException();
        }
        await repository.DeleteAsync(command.Id, cancellationToken);
        return true;
    }
}
