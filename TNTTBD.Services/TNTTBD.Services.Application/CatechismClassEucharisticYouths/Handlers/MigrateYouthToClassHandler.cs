using MediatR;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.Commands;
using TNTTBD.Services.Domain.Catechism.JoinModels;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.CatechismClassEucharisticYouths.Handlers;

public class MigrateYouthToClassHandler : IRequestHandler<MigrateYouthToClassCommand, int>
{
    private readonly ICatechismClassEucharisticYouthRepository _repository;

    public MigrateYouthToClassHandler(ICatechismClassEucharisticYouthRepository repository)
    {
        _repository = repository;
    }

    public async Task<int> Handle(MigrateYouthToClassCommand command, CancellationToken cancellationToken)
    {
        var activeIds = await _repository.GetActiveEnrollmentIdsAsync(
            command.SourceClassId, command.EucharisticYouthIds, cancellationToken);

        if (activeIds.Count == 0) return 0;

        await _repository.MarkCompleted(activeIds, cancellationToken);

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

            await _repository.AddAsync(enrollment, cancellationToken);
        }

        return activeIds.Count;
    }
}
