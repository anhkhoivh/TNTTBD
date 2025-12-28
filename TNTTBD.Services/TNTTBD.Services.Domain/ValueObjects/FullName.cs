using TNTTBD.Services.Domain.Common;

namespace TNTTBD.Services.Domain.ValueObjects;

/// <summary>
/// Full name
/// </summary>
public class FullName : ValueObject
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public FullName() {}

    /// <summary>
    /// Constructor from params
    /// </summary>
    /// <param name="familyName"></param>
    /// <param name="middleName"></param>
    /// <param name="givenName"></param>
    /// <exception cref="DomainException"></exception>
    public FullName(string familyName, string middleName, string givenName)
    {
        if (string.IsNullOrWhiteSpace(familyName))
            throw new DomainException("Family name is required.");

        FamilyName = familyName;
        MiddleName = middleName ?? string.Empty;
        GivenName = givenName;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Family Name
    /// </summary>
    public string FamilyName { get; set; }

    /// <summary>
    /// Middle Name
    /// </summary>
    public string MiddleName { get; set; }

    /// <summary>
    /// Given Name
    /// </summary>
    public string GivenName { get; set; }

    #endregion

    #region Methods

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FamilyName;
        yield return MiddleName;
        yield return GivenName;
    }

    public string Display() => $"{FamilyName} {MiddleName} {GivenName}";

    #endregion
}