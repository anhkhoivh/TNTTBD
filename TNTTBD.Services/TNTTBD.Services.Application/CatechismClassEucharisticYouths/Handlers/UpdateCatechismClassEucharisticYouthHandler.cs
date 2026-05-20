using MediatR;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.Commands;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.CatechismClassEucharisticYouths.Handlers;

/// <summary>
/// Create Catechist handler
/// </summary>
public class UpdateCatechismClassEucharisticYouthHandler : IRequestHandler<UpdateCatechismClassEucharisticYouthCommand>
{
    #region Fields

    private readonly ICatechismClassEucharisticYouthRepository _repository;

    #endregion

    #region Constructors

    public UpdateCatechismClassEucharisticYouthHandler(ICatechismClassEucharisticYouthRepository repository)
    {
        _repository = repository;
    }

    #endregion

    #region Methods

    public async Task Handle(UpdateCatechismClassEucharisticYouthCommand command, CancellationToken cancellationToken)
    {
        await _repository.Update(command.Id, command.Status, command.Note, cancellationToken);
    }

    #endregion
}