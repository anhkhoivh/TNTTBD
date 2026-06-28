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
    public void UpdateMilestone(Guid milestoneId, DateOnly date, string? documentCode, string? place, string? note)
    {
        SpiritualMilestone milestone = _milestones
            .FirstOrDefault(x => x.Id == milestoneId)
            ?? throw new DomainException(DomainErrors.ValidationNotFound);

        milestone.Update(date, documentCode, place, note);
    }

    /// <summary>
    /// Remove milestone
    /// </summary>
    public void RemoveMilestone(Guid milestoneId)
    {
        SpiritualMilestone milestone = _milestones
            .FirstOrDefault(x => x.Id == milestoneId)
            ?? throw new DomainException(DomainErrors.ValidationNotFound);
        _milestones.Remove(milestone);
    }

    #endregion
}
