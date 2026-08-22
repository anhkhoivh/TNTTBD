using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.Commands;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.DTOs;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.Interfaces;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.CatechismClassEucharisticYouths.Handlers;

/// <summary>
/// Create Catechist handler
/// </summary>
public class UpdateCatechismClassEucharisticYouthHandler(
    ICatechismClassEucharisticYouthRepository repository, 
    IAuthorizationContext authorizationContext,
    ICatechismClassEucharisticYouthQueryService queryService) : IRequestHandler<UpdateCatechismClassEucharisticYouthCommand, bool>
{
    #region Methods

    public async Task<bool> Handle(UpdateCatechismClassEucharisticYouthCommand command, CancellationToken cancellationToken)
    {
        CatechismClassEucharisticYouthDto? catechismClassEucharisticYouth = await queryService.GetByIdAsync(command.Id, cancellationToken);
        if (catechismClassEucharisticYouth == null || 
            !await authorizationContext.CanAccessClassAsync(catechismClassEucharisticYouth.CatechismClassId, cancellationToken))
        {
            return false;
        }
        await repository.Update(command.Id, command.Status, command.Note, cancellationToken);
        return true;
    }

    #endregion
}