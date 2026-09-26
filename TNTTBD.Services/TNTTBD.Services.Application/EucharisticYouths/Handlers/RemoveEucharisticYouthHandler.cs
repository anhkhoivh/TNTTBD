using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public class RemoveEucharisticYouthHandler : IRequestHandler<RemoveEucharisticYouthCommand, bool>
{
    private readonly IEucharisticYouthRepository _repository;
    private readonly IAuthorizationContext _authContext;

    public RemoveEucharisticYouthHandler(IEucharisticYouthRepository repository, IAuthorizationContext authContext)
    {
        _repository = repository;
        _authContext = authContext;
    }

    public async Task<bool> Handle(RemoveEucharisticYouthCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.IsAdminAsync(cancellationToken))
        {
            return false;
        }

        // Linked User (if any) is hard-deleted by the DB via cascade FK (UserEntityConfiguration.EucharisticYouthId).
        await _repository.Remove(command.EucharisticYouthId, cancellationToken);
        return true;
    }
}