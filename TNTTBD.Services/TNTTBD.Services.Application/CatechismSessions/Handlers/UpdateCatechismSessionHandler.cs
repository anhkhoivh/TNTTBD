using MediatR;
using TNTTBD.Services.Application.CatechismClasses.Commands;
using TNTTBD.Services.Application.CatechismSessions.Commands;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.CatechismSessions.Handlers;

/// <summary>
/// Create Catechist handler
/// </summary>
public class UpdateCatechismSessionHandler : IRequestHandler<UpdateCatechismSessionCommand>
{
    #region Fields

    private readonly ICatechismSessionRepository _repository;

    #endregion

    #region Constructors

    public UpdateCatechismSessionHandler(ICatechismSessionRepository repository)
    {
        _repository = repository;
    }

    #endregion

    #region Methods

    public async Task Handle(UpdateCatechismSessionCommand command, CancellationToken cancellationToken)
    {
        await _repository.Update(command.CatechismSessionId, command.DateTime, command.Title, cancellationToken);
    }

    #endregion
}