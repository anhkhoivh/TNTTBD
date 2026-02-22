using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Domain.Catechism.Members;

/// <summary>
/// Student entity
/// </summary>
public class EucharisticYouth : Member
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public EucharisticYouth() : base()
    {
        Address = string.Empty;
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    public EucharisticYouth(Guid id, FullName name, DateOnly dateOfBirth, string? address, IEnumerable<PhotoReference> photos, IEnumerable<SpiritualMilestone> milestones, IEnumerable<PatronSaint> patronSaints) : base(id, name, dateOfBirth, photos, milestones, patronSaints)
    {
        Address = address ?? string.Empty;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Address
    /// </summary>
    public string Address { get; private set; }

    #endregion
}
