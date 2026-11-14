using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Domain.Catechism;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Common;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Mappers;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Repositories;

public class MilestoneEventRepository : IMilestoneEventRepository
{
    private readonly CatechismDbContext _context;

    public MilestoneEventRepository(CatechismDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<MilestoneEvent?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        MilestoneEventEntity? entity = await _context.MilestoneEvents
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        return entity == null
            ? null
            : new MilestoneEvent(entity.Id, entity.Name, entity.Type, entity.Date, entity.Place, entity.Celebrant, entity.Note, entity.CodeTemplate, entity.StartIndex);
    }

    /// <inheritdoc />
    public Task<int> CountParticipantsAsync(Guid id, CancellationToken cancellationToken)
    {
        return _context.SpiritualMilestones.CountAsync(m => m.MilestoneEventId == id, cancellationToken);
    }

    /// <inheritdoc />
    public async Task SaveAsync(
        MilestoneEvent milestoneEvent,
        bool isNew,
        IReadOnlyList<MilestoneEventParticipant> participants,
        Guid? userId,
        CancellationToken cancellationToken)
    {
        DateTime now = DateTime.UtcNow;
        MilestoneEventEntity eventEntity;
        if (isNew)
        {
            eventEntity = new MilestoneEventEntity { Id = milestoneEvent.Id, CreatedAt = now, CreatedBy = userId };
            await _context.MilestoneEvents.AddAsync(eventEntity, cancellationToken);
        }
        else
        {
            eventEntity = await _context.MilestoneEvents.FirstOrDefaultAsync(e => e.Id == milestoneEvent.Id, cancellationToken)
                ?? throw new DomainException(DomainErrors.ValidationNotFound);
        }

        eventEntity.Name = milestoneEvent.Name;
        eventEntity.Type = milestoneEvent.Type;
        eventEntity.Date = milestoneEvent.Date;
        eventEntity.Place = milestoneEvent.Place;
        eventEntity.Celebrant = milestoneEvent.Celebrant;
        eventEntity.Note = milestoneEvent.Note;
        eventEntity.CodeTemplate = milestoneEvent.CodeTemplate;
        eventEntity.StartIndex = milestoneEvent.StartIndex;
        eventEntity.UpdatedAt = now;
        eventEntity.UpdatedBy = userId;

        var previouslyLinked = isNew
            ? []
            : await _context.SpiritualMilestones
                .Where(m => m.MilestoneEventId == milestoneEvent.Id)
                .Select(m => new { m.Id, m.EucharisticYouthId })
                .ToListAsync(cancellationToken);

        HashSet<Guid> requestedYouthIds = participants.Select(p => p.EucharisticYouthId).ToHashSet();
        List<Guid> allYouthIds = requestedYouthIds
            .Union(previouslyLinked.Select(p => p.EucharisticYouthId))
            .ToList();

        Dictionary<Guid, EucharisticYouthEntity> youths = await _context.EucharisticYouths
            .Include(y => y.SpiritualMilestones)
            .Where(y => allYouthIds.Contains(y.Id))
            .ToDictionaryAsync(y => y.Id, cancellationToken);

        if (requestedYouthIds.Any(id => !youths.ContainsKey(id)))
        {
            throw new DomainException(DomainErrors.ValidationNotFound);
        }

        // Removed participants: unlink only, the milestone itself is kept.
        foreach (var removed in previouslyLinked.Where(p => !requestedYouthIds.Contains(p.EucharisticYouthId)))
        {
            EucharisticYouthEntity youthEntity = youths[removed.EucharisticYouthId];
            EucharisticYouth youth = youthEntity.ToDomain();
            youth.UnlinkMilestoneFromEvent(removed.Id);
            youth.UpdateEntity(youthEntity);
        }

        foreach (MilestoneEventParticipant p in participants)
        {
            EucharisticYouthEntity youthEntity = youths[p.EucharisticYouthId];
            EucharisticYouth youth = youthEntity.ToDomain();
            DateOnly date = p.Date ?? milestoneEvent.Date;
            string? place = p.Place ?? milestoneEvent.Place;
            string? celebrant = p.Celebrant ?? milestoneEvent.Celebrant;

            SpiritualMilestone? existing = youth.Milestones.FirstOrDefault(m => m.Type == milestoneEvent.Type);
            Guid? newMilestoneId = null;
            if (existing == null)
            {
                newMilestoneId = Guid.NewGuid();
                youth.AddMilestone(new SpiritualMilestone(
                    newMilestoneId.Value,
                    milestoneEvent.Type,
                    date,
                    p.DocumentCode,
                    place,
                    p.Note,
                    p.SponsorContactId,
                    celebrant,
                    milestoneEvent.Id));
            }
            else
            {
                if (existing.MilestoneEventId.HasValue && existing.MilestoneEventId != milestoneEvent.Id)
                {
                    throw new DomainException(DomainErrors.MilestoneEventConflict);
                }
                if (!existing.MilestoneEventId.HasValue && p.MilestoneId != existing.Id)
                {
                    // A standalone milestone exists; the admin must explicitly choose to link it.
                    throw new DomainException(DomainErrors.MilestoneEventUnlinkedExisting);
                }
                youth.UpdateMilestone(existing.Id, date, p.DocumentCode, place, p.Note, p.SponsorContactId, celebrant);
                youth.LinkMilestoneToEvent(existing.Id, milestoneEvent.Id);
            }

            youth.UpdateEntity(youthEntity);

            if (newMilestoneId.HasValue)
            {
                SpiritualMilestoneEntity added = youthEntity.SpiritualMilestones.First(m => m.Id == newMilestoneId.Value);
                _context.Entry(added).State = EntityState.Added;
            }
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        MilestoneEventEntity entity = await _context.MilestoneEvents.FirstOrDefaultAsync(e => e.Id == id, cancellationToken)
            ?? throw new DomainException(DomainErrors.ValidationNotFound);
        if (await CountParticipantsAsync(id, cancellationToken) > 0)
        {
            throw new DomainException(DomainErrors.MilestoneEventNotEmpty);
        }
        _context.MilestoneEvents.Remove(entity);
    }
}
