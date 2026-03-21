using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public class RemoveEucharisticYouthPatronSaintHandler : IRequestHandler<RemoveEucharisticYouthPatronSaintCommand>
{
    private readonly IEucharisticYouthRepository _repository;

    public RemoveEucharisticYouthPatronSaintHandler(IEucharisticYouthRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(RemoveEucharisticYouthPatronSaintCommand command, CancellationToken cancellationToken)
    {
        PatronSaint patronSaint = new PatronSaint(command.SaintName, command.Day, command.Month);
        await _repository.RemovePatronSaint(command.EucharisticYouthId, patronSaint, cancellationToken);
    }
}