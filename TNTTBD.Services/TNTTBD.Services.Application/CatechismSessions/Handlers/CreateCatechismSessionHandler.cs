using MediatR;
using TNTTBD.Services.Application.CatechismClasses.Commands;
using TNTTBD.Services.Application.CatechismSessions.Commands;
using TNTTBD.Services.Domain.Catechism;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.CatechismSessions.Handlers;

/// <summary>
/// Create Session handler
/// </summary>
public class CreateCatechismSessionHandler : IRequestHandler<CreateCatechismSessionCommand, Guid>
{
    #region Fields

    private readonly ICatechismSessionRepository _repository;

    #endregion

    #region Constructors

    public CreateCatechismSessionHandler(ICatechismSessionRepository repository)
    {
        _repository = repository;
    }

    #endregion

    #region Methods

    public async Task<Guid> Handle(CreateCatechismSessionCommand command, CancellationToken cancellationToken)
    {
        CatechismSession session = new(Guid.NewGuid(), command.CatechismClassId, command.DateTime, command.Title);
        await _repository.AddAsync(session, cancellationToken);
        return session.Id;
    }

    #endregion
}