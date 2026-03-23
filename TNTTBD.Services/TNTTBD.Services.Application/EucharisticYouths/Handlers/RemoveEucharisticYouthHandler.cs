using MediatR;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public class RemoveEucharisticYouthHandler : IRequestHandler<RemoveEucharisticYouthCommand>
{
    private readonly IEucharisticYouthRepository _repository;

    public RemoveEucharisticYouthHandler(IEucharisticYouthRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(RemoveEucharisticYouthCommand command, CancellationToken cancellationToken)
    {
        await _repository.Remove(command.EucharisticYouthId, cancellationToken);
    }
}