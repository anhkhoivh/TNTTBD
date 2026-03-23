using MediatR;
using TNTTBD.Services.Application.CatechismYears.Commands;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.CatechismYears.Handlers;

/// <summary>
/// Create Catechist handler
/// </summary>
public class UpdateCatechismYearHandler : IRequestHandler<UpdateCatechismYearCommand>
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

    public async Task Handle(UpdateCatechismYearCommand command, CancellationToken cancellationToken)
    {
        await _repository.Update(command.CatechismYearId, command.StartDate, command.EndDate, command.Theme, cancellationToken);
    }

    #endregion
}