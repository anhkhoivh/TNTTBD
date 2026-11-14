using TNTTBD.Services.Domain.Common;
using TNTTBD.Services.Domain.Enums;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Domain.Catechism.Members;

/// <summary>
/// Student entity
/// </summary>
public class EucharisticYouth : Member
{
    #region Fields

    private readonly List<SpiritualMilestone> _milestones = new List<SpiritualMilestone>();

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public EucharisticYouth() : base()
    {
        Code = string.Empty;
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    public EucharisticYouth(Guid id, string code, FullName name, DateOnly? dateOfBirth, string? address, bool isMale) : base(id, name, dateOfBirth, address, isMale)
    {
        Code = code;
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    public EucharisticYouth(Guid id, string code, FullName name, DateOnly? dateOfBirth, string? address, bool isMale, IEnumerable<PhotoReference> photos, IEnumerable<SpiritualMilestone> milestones, IEnumerable<PatronSaint> patronSaints) : base(id, name, dateOfBirth, address, isMale, photos, patronSaints)
    {
        Code = code;
        _milestones.AddRange(milestones);
    }

    #endregion

    #region Properties

    /// <summary>
    /// Eucharistic Youth Code
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Milestone
    /// </summary>
    public IReadOnlyCollection<SpiritualMilestone> Milestones => _milestones.AsReadOnly();

    #endregion

    #region Methods

    /// <summary>
    /// Replace the milestone list from a profile edit. Milestones are matched by type so that
    /// existing records (and their event links) are updated in place.
    /// </summary>
    public void UpdateMilestoneList(IReadOnlyCollection<SpiritualMilestone> milestones)
    {
        if (milestones.Count > 0 && milestones.Any(m => m.Type == MilestoneType.Baptism) == false)
        {
            throw new DomainException(DomainErrors.NotBaptised);
        }
        if (milestones.GroupBy(m => m.Type).Any(g => g.Count() > 1))
        {
            throw new DomainException(DomainErrors.MilestoneAlreadyExists);
        }

        List<SpiritualMilestone> removed = _milestones
            .Where(existing => milestones.All(m => m.Type != existing.Type))
            .ToList();
        if (removed.Any(m => m.MilestoneEventId.HasValue))
        {
            throw new DomainException(DomainErrors.MilestoneLinkedToEvent);
        }
        foreach (SpiritualMilestone r in removed)
        {
            _milestones.Remove(r);
        }

        foreach (SpiritualMilestone incoming in milestones)
        {
            SpiritualMilestone? existing = _milestones.FirstOrDefault(m => m.Type == incoming.Type);
            if (existing != null)
            {
                existing.Update(incoming.Date, incoming.DocumentCode, incoming.Place, incoming.Note, incoming.SponsorContactId, incoming.Celebrant);
            }
            else
            {
                _milestones.Add(new SpiritualMilestone(
                    incoming.Id == Guid.Empty ? Guid.NewGuid() : incoming.Id,
                    incoming.Type,
                    incoming.Date,
                    incoming.DocumentCode,
                    incoming.Place,
                    incoming.Note,
                    incoming.SponsorContactId,
                    incoming.Celebrant));
            }
        }
    }

    /// <summary>
    /// Add milestone
    /// </summary>
    public void AddMilestone(SpiritualMilestone milestone)
    {
        if (milestone.Type != MilestoneType.Baptism && _milestones.Any(m => m.Type == MilestoneType.Baptism) == false)
        {
            throw new DomainException(DomainErrors.NotBaptised);
        }
        if (_milestones.Any(m => m.Type == milestone.Type))
        {
            throw new DomainException(DomainErrors.MilestoneAlreadyExists);
        }

        _milestones.Add(milestone);
    }

    /// <summary>
    /// Update
    /// </summary>
    public void UpdateMilestone(Guid milestoneId, DateOnly? date, string? documentCode, string? place, string? note, Guid? sponsorContactId, string? celebrant)
    {
        GetMilestone(milestoneId).Update(date, documentCode, place, note, sponsorContactId, celebrant);
    }

    /// <summary>
    /// Link a milestone to a group sacrament event
    /// </summary>
    public void LinkMilestoneToEvent(Guid milestoneId, Guid milestoneEventId)
    {
        GetMilestone(milestoneId).LinkToEvent(milestoneEventId);
    }

    /// <summary>
    /// Unlink a milestone from its event (the milestone itself is kept)
    /// </summary>
    public void UnlinkMilestoneFromEvent(Guid milestoneId)
    {
        GetMilestone(milestoneId).UnlinkFromEvent();
    }

    /// <summary>
    /// Remove milestone
    /// </summary>
    public void RemoveMilestone(Guid milestoneId)
    {
        SpiritualMilestone milestone = GetMilestone(milestoneId);
        if (milestone.MilestoneEventId.HasValue)
        {
            throw new DomainException(DomainErrors.MilestoneLinkedToEvent);
        }
        _milestones.Remove(milestone);
    }

    private SpiritualMilestone GetMilestone(Guid milestoneId) =>
        _milestones.FirstOrDefault(x => x.Id == milestoneId)
        ?? throw new DomainException(DomainErrors.ValidationNotFound);

    #endregion
}
