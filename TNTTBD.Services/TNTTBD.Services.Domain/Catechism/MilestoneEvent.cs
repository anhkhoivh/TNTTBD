using TNTTBD.Services.Domain.Common;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Domain.Catechism;

/// <summary>
/// A group sacrament Mass in which several youths receive the same milestone.
/// Participants are the <see cref="Members.SpiritualMilestone"/> records linked to this event.
/// </summary>
public class MilestoneEvent : AggregateRoot<Guid>
{
    public const int MaxNameLength = 200;
    public const int MaxPlaceLength = 200;
    public const int MaxNoteLength = 1000;
    public const int MaxCelebrantLength = 200;

    #region Constructors

    public MilestoneEvent(
        Guid id,
        string name,
        MilestoneType type,
        DateOnly date,
        string? place,
        string? celebrant,
        string? note,
        string? codeTemplate,
        int startIndex) : base(id)
    {
        Type = type;
        SetDetails(name, date, place, celebrant, note, codeTemplate, startIndex);
    }

    #endregion

    #region Properties

    public string Name { get; private set; } = string.Empty;

    public MilestoneType Type { get; private set; }

    public DateOnly Date { get; private set; }

    public string? Place { get; private set; }

    /// <summary>
    /// Bishop / priest who celebrated the Mass (default for participants)
    /// </summary>
    public string? Celebrant { get; private set; }

    public string? Note { get; private set; }

    public string? CodeTemplate { get; private set; }

    public int StartIndex { get; private set; }

    #endregion

    #region Methods

    /// <summary>
    /// Update the event header. The type may only change while the event has no participants.
    /// </summary>
    public void Update(string name, MilestoneType type, DateOnly date, string? place, string? celebrant, string? note, string? codeTemplate, int startIndex, bool hasParticipants)
    {
        if (type != Type && hasParticipants)
        {
            throw new DomainException(DomainErrors.MilestoneEventTypeLocked);
        }
        Type = type;
        SetDetails(name, date, place, celebrant, note, codeTemplate, startIndex);
    }

    private void SetDetails(string name, DateOnly date, string? place, string? celebrant, string? note, string? codeTemplate, int startIndex)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException(DomainErrors.ValidationRequired);
        }
        if (startIndex < 0)
        {
            throw new DomainException(DomainErrors.MilestoneEventInvalidTemplate);
        }
        Name = Truncate(name.Trim(), MaxNameLength)!;
        Date = date;
        Place = Truncate(string.IsNullOrWhiteSpace(place) ? null : place.Trim(), MaxPlaceLength);
        Celebrant = Truncate(string.IsNullOrWhiteSpace(celebrant) ? null : celebrant.Trim(), MaxCelebrantLength);
        Note = Truncate(string.IsNullOrWhiteSpace(note) ? null : note.Trim(), MaxNoteLength);
        CodeTemplate = string.IsNullOrWhiteSpace(codeTemplate) ? null : codeTemplate.Trim();
        StartIndex = startIndex;
    }

    private static string? Truncate(string? value, int max) =>
        value != null && value.Length > max ? value[..max] : value;

    #endregion
}
