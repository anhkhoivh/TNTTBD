using MediatR;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.Commands;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.CatechismClassEucharisticYouths.Handlers;

/// <summary>
/// Create Catechist handler
/// </summary>
public class RemoveEucharisticYouthFromClassHandler : IRequestHandler<RemoveEucharisticYouthFromClassCommand>
{
    #region Fields

    private readonly ICatechismClassEucharisticYouthRepository _repository;

    #endregion

    #region Constructors

    public RemoveEucharisticYouthFromClassHandler(ICatechismClassEucharisticYouthRepository repository)
    {
        _repository = repository;
    }

    #endregion

    #region Methods

    public async Task Handle(RemoveEucharisticYouthFromClassCommand command, CancellationToken cancellationToken)
    {
        await _repository.Remove(command.Id, cancellationToken);
    }

    #endregion
}