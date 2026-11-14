using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Domain.Catechism.Repositories;

/// <summary>
/// One participant of a milestone event save. Null <see cref="Date"/> / <see cref="Place"/> / <see cref="Celebrant"/> fall back to the event's values.
/// <see cref="MilestoneId"/> must be provided to link an existing standalone milestone of the event type.
/// </summary>
public record MilestoneEventParticipant(
    Guid EucharisticYouthId,
    Guid? MilestoneId,
    DateOnly? Date,
    string? Place,
    string? DocumentCode,
    Guid? SponsorContactId,
    string? Celebrant,
    string? Note);

public interface IMilestoneEventRepository
{
    Task<MilestoneEvent?> GetAsync(Guid id, CancellationToken cancellationToken);

    Task<int> CountParticipantsAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Persist the event header and synchronise its participants (create/update/link milestones,
    /// unlink removed participants without deleting their milestones).
    /// </summary>
    Task SaveAsync(
        MilestoneEvent milestoneEvent,
        bool isNew,
        IReadOnlyList<MilestoneEventParticipant> participants,
        Guid? userId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Delete an event that has no participants
    /// </summary>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
