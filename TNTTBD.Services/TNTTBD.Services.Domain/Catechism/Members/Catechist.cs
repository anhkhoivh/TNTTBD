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
    public Catechist(Guid id, FullName name, DateOnly? dateOfBirth, string? address, bool isMale, string? phoneNumber = null) : base(id, name, dateOfBirth, address, isMale, phoneNumber)
    {
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    public Catechist(Guid id, FullName name, DateOnly? dateOfBirth, string? address, bool isMale, IEnumerable<PhotoReference> photos, IEnumerable<PatronSaint> patronSaints, string? phoneNumber = null) : base(id, name, dateOfBirth, address, isMale, photos, patronSaints, phoneNumber)
    {
    }

    #endregion

    #region Properties

    #endregion

}
