using MediatR;
using TNTTBD.Services.Application.CatechismClasses.Commands;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.CatechismClasses.Handlers;

/// <summary>
/// Create Catechist handler
/// </summary>
public class UpdateCatechismYearHandler : IRequestHandler<UpdateCatechismClassCommand>
{
    #region Fields

    private readonly ICatechismYearRepository _repository;

    #endregion

    #region Constructors

    public UpdateCatechismYearHandler(ICatechismYearRepository repository)
    {
        _repository = repository;
    }

    #endregion

    #region Methods

    public async Task Handle(UpdateCatechismClassCommand command, CancellationToken cancellationToken)
    {
        await _repository.Update(command.CatechismClassId, command.StartDate, command.EndDate, command.Theme, cancellationToken);
    }

    #endregion
}