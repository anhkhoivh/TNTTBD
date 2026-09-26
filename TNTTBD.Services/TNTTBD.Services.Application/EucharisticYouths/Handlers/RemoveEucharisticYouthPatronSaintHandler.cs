using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public class RemoveEucharisticYouthPatronSaintHandler : IRequestHandler<RemoveEucharisticYouthPatronSaintCommand, bool>
{
    private readonly IEucharisticYouthRepository _repository;
    private readonly IAuthorizationContext _authContext;

    public RemoveEucharisticYouthPatronSaintHandler(IEucharisticYouthRepository repository, IAuthorizationContext authContext)
    {
        _repository = repository;
        _authContext = authContext;
    }

    public async Task<bool> Handle(RemoveEucharisticYouthPatronSaintCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanAccessYouthAsync(command.EucharisticYouthId, cancellationToken))
        {
            return false;
        }

        PatronSaint patronSaint = new PatronSaint(command.SaintName, command.Day, command.Month);
        await _repository.RemovePatronSaint(command.EucharisticYouthId, patronSaint, cancellationToken);
        return true;
    }
}