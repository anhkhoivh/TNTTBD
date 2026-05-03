using MediatR;
using TNTTBD.Services.Application.CatechismClasses.Commands;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.CatechismClasses.Handlers;

/// <summary>
/// Create Catechist handler
/// </summary>
public class UpdateCatechismClassHandler : IRequestHandler<UpdateCatechismClassCommand>
{
    #region Fields

    private readonly ICatechismClassRepository _repository;

    #endregion

    #region Constructors

    public UpdateCatechismClassHandler(ICatechismClassRepository repository)
    {
        _repository = repository;
    }

    #endregion

    #region Methods

    public async Task Handle(UpdateCatechismClassCommand command, CancellationToken cancellationToken)
    {
        await _repository.Update(command.CatechismClassId, command.ClassName, cancellationToken);
    }

    #endregion
}