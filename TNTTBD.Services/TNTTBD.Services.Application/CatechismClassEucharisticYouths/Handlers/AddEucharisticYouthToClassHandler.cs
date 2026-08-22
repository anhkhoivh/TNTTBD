using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.Commands;
using TNTTBD.Services.Domain.Catechism.JoinModels;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.CatechismClassEucharisticYouths.Handlers;

/// <summary>
/// Create Catechist handler
/// </summary>
public class AddEucharisticYouthToClassHandler(ICatechismClassEucharisticYouthRepository repository, IAuthorizationContext authorizationContext) : IRequestHandler<AddEucharisticYouthToClassCommand, Guid?>
{
    #region Methods

    public async Task<Guid?> Handle(AddEucharisticYouthToClassCommand command, CancellationToken cancellationToken)
    {
        if (!await authorizationContext.CanAccessClassAsync(command.CatechismClassId, cancellationToken))
        {
            return null;
        }
        CatechismClassEucharisticYouth domain = new(Guid.NewGuid(), command.EucharisticYouthId, command.CatechismClassId, DateTimeOffset.Now.ToUniversalTime(), null, EnrollmentStatus.Active, null);
        await repository.AddAsync(domain, cancellationToken);
        return domain.Id;
    }

    #endregion
}