using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Domain.Catechism.Members;

/// <summary>
/// Student entity
/// </summary>
public class Catechist : Member
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public Catechist() : base()
    {
        FullName = new FullName();
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    public Catechist(Guid id, FullName name, DateOnly dateOfBirth, string? address, bool isMale, IEnumerable<PhotoReference> photos, IEnumerable<SpiritualMilestone> milestones, IEnumerable<PatronSaint> patronSaints) : base(id, name, dateOfBirth, address, isMale, photos, milestones, patronSaints)
    {
    }

    #endregion

    #region Properties

    #endregion

}
