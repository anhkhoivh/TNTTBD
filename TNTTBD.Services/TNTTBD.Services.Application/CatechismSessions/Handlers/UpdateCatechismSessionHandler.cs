using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.CatechismClasses.Commands;
using TNTTBD.Services.Application.CatechismSessions.Commands;
using TNTTBD.Services.Application.CatechismSessions.DTOs;
using TNTTBD.Services.Application.CatechismSessions.Interfaces;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.CatechismSessions.Handlers;

/// <summary>
/// Create Catechist handler
/// </summary>
public class UpdateCatechismSessionHandler(
    ICatechismSessionRepository repository, 
    IAuthorizationContext authorizationContext,
    ICatechismSessionQueryService queryService) : IRequestHandler<UpdateCatechismSessionCommand, bool>
{
    #region Methods

    public async Task<bool> Handle(UpdateCatechismSessionCommand command, CancellationToken cancellationToken)
    {
        CatechismSessionDto? session = await queryService.GetByIdAsync(command.CatechismSessionId, cancellationToken);
        if (session == null ||
            !await authorizationContext.CanAccessClassAsync(session.CatechismClassId, cancellationToken))
        {
            return false;
        }
        await repository.Update(command.CatechismSessionId, command.DateTime, command.Title, cancellationToken);
        return true;
    }

    #endregion
}