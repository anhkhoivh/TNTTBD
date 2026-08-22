using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.Commands;
using TNTTBD.Services.Domain.Catechism.JoinModels;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.CatechismClassEucharisticYouths.Handlers;

public class MigrateYouthToClassHandler(ICatechismClassEucharisticYouthRepository repository, IAuthorizationContext authorizationContext) : IRequestHandler<MigrateYouthToClassCommand, int>
{
    public async Task<int> Handle(MigrateYouthToClassCommand command, CancellationToken cancellationToken)
    {
        if (!await authorizationContext.CanAccessClassAsync(command.SourceClassId, cancellationToken))
        {
            return -1;
        }
        var activeIds = await repository.GetActiveEnrollmentIdsAsync(
            command.SourceClassId, command.EucharisticYouthIds, cancellationToken);

        if (activeIds.Count == 0)
        {
           return 0; 
        } 

        await repository.MarkCompleted(activeIds, cancellationToken);

        foreach (var youthId in command.EucharisticYouthIds)
        {
            var enrollment = new CatechismClassEucharisticYouth(
                Guid.NewGuid(),
                youthId,
                command.DestinationClassId,
                DateTimeOffset.UtcNow.ToUniversalTime(),
                null,
                EnrollmentStatus.Active,
                null);

            await repository.AddAsync(enrollment, cancellationToken);
        }

        return activeIds.Count;
    }
}
