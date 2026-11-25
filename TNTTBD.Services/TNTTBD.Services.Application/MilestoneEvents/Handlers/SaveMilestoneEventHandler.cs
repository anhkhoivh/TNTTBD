using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Common.Exceptions;
using TNTTBD.Services.Application.MilestoneEvents.Commands;
using TNTTBD.Services.Application.MilestoneEvents.DTOs;
using TNTTBD.Services.Domain.Catechism;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Common;

namespace TNTTBD.Services.Application.MilestoneEvents.Handlers;

public class SaveMilestoneEventHandler(
    IMilestoneEventRepository repository,
    IEucharisticYouthContactRepository youthContactRepository,
    IAuthorizationContext auth) : IRequestHandler<SaveMilestoneEventCommand, Guid>
{
    public const int MaxParticipants = 1000;

    public async Task<Guid> Handle(SaveMilestoneEventCommand command, CancellationToken cancellationToken)
    {
        if (!await auth.IsAdminOrCatechistAsync(cancellationToken))
        {
            throw new ForbiddenException();
        }

        List<SaveMilestoneEventParticipantDto> rows = command.Participants ?? [];
        if (rows.Count > MaxParticipants)
        {
            throw new DomainException(DomainErrors.ValidationRequired);
        }
        if (rows.GroupBy(p => p.EucharisticYouthId).Any(g => g.Count() > 1))
        {
            throw new DomainException(DomainErrors.MilestoneEventDuplicateYouth);
        }
        if (!string.IsNullOrWhiteSpace(command.CodeTemplate))
        {
            MilestoneCodeTemplate.Parse(command.CodeTemplate);
        }

        MilestoneEvent milestoneEvent;
        bool isNew = command.Id == null;
        if (isNew)
        {
            milestoneEvent = new MilestoneEvent(Guid.NewGuid(), command.Name, command.Type, command.Date, command.Place, command.Celebrant, command.Note, command.CodeTemplate, command.StartIndex);
        }
        else
        {
            milestoneEvent = await repository.GetAsync(command.Id!.Value, cancellationToken)
                ?? throw new DomainException(DomainErrors.ValidationNotFound);
            int count = await repository.CountParticipantsAsync(milestoneEvent.Id, cancellationToken);
            milestoneEvent.Update(command.Name, command.Type, command.Date, command.Place, command.Celebrant, command.Note, command.CodeTemplate, command.StartIndex, count > 0);
        }

        List<MilestoneEventParticipant> participants = rows
            .Select(p => new MilestoneEventParticipant(
                p.EucharisticYouthId,
                p.MilestoneId,
                p.Date,
                string.IsNullOrWhiteSpace(p.Place) ? null : p.Place.Trim(),
                p.DocumentCode,
                p.SponsorContactId,
                string.IsNullOrWhiteSpace(p.Celebrant) ? null : p.Celebrant.Trim(),
                string.IsNullOrWhiteSpace(p.Note) ? null : p.Note.Trim()))
            .ToList();

        await repository.SaveAsync(milestoneEvent, isNew, participants, auth.GetCurrentUserId(), cancellationToken);
        foreach (MilestoneEventParticipant p in participants)
        {
            await youthContactRepository.LinkGodparentAsync(p.EucharisticYouthId, milestoneEvent.Type, p.SponsorContactId, cancellationToken);
        }
        return milestoneEvent.Id;
    }
}
