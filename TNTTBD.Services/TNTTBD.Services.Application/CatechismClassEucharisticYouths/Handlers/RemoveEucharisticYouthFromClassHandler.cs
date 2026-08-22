using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.Commands;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.DTOs;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.Interfaces;
using TNTTBD.Services.Domain.Catechism.JoinModels;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.CatechismClassEucharisticYouths.Handlers;

/// <summary>
/// Create Catechist handler
/// </summary>
public class RemoveEucharisticYouthFromClassHandler(
    ICatechismClassEucharisticYouthRepository repository,
    IAuthorizationContext authorizationContext,
    ICatechismClassEucharisticYouthQueryService queryService) : IRequestHandler<RemoveEucharisticYouthFromClassCommand, bool>
{
    #region Methods

    public async Task<bool> Handle(RemoveEucharisticYouthFromClassCommand command, CancellationToken cancellationToken)
    {
        CatechismClassEucharisticYouthDto? catechismClassEucharisticYouth = await queryService.GetByIdAsync(command.Id, cancellationToken);
        if (catechismClassEucharisticYouth == null || 
            !await authorizationContext.CanAccessClassAsync(catechismClassEucharisticYouth.CatechismClassId, cancellationToken))
        {
            return false;
        }
        await repository.Remove(command.Id, cancellationToken);
        return true;
    }

    #endregion
}