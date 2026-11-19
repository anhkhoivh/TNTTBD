using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.Commands;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.CatechismClassEucharisticYouths.Handlers;

/// <summary>
/// Create Catechist handler
/// </summary>
public class MarkCatechismClassEucharisticYouthsCompletedHandler : IRequestHandler<MarkCatechismClassEucharisticYouthsCompletedCommand>
{
    #region Fields

    private readonly ICatechismClassEucharisticYouthRepository _repository;

    #endregion

    #region Constructors

    public MarkCatechismClassEucharisticYouthsCompletedHandler(ICatechismClassEucharisticYouthRepository repository)
    {
        _repository = repository;
    }

    #endregion

    #region Methods

    public async Task Handle(MarkCatechismClassEucharisticYouthsCompletedCommand command, CancellationToken cancellationToken)
    {
        await _repository.MarkCompleted(command.Ids, cancellationToken);
    }

    #endregion
}