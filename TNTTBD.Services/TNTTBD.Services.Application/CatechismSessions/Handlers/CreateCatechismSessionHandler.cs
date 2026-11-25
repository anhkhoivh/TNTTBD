using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.CatechismClasses.Commands;
using TNTTBD.Services.Application.CatechismSessions.Commands;
using TNTTBD.Services.Application.Common.Exceptions;
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
    private readonly IAuthorizationContext _authContext;

    #endregion

    #region Constructors

    public CreateCatechismSessionHandler(ICatechismSessionRepository repository, IAuthorizationContext authContext)
    {
        _repository = repository;
        _authContext = authContext;
    }

    #endregion

    #region Methods

    public async Task<Guid> Handle(CreateCatechismSessionCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanAccessClassAsync(command.CatechismClassId, cancellationToken))
        {
            throw new ForbiddenException();
        }

        CatechismSession session = new(Guid.NewGuid(), command.CatechismClassId, command.DateTime, command.Title);
        await _repository.AddAsync(session, cancellationToken);
        return session.Id;
    }

    #endregion
}