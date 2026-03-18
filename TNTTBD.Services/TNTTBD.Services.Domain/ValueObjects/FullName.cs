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
    public FullName()
    {
        FamilyName = string.Empty;
        GivenName = string.Empty;
        MiddleName = null;
    }

    /// <summary>
    /// Constructor from params
    /// </summary>
    public FullName(string givenName, string? christName, string? familyName = null , string? middleName = null)
    {
        ChristName = string.IsNullOrWhiteSpace(christName) ? null : Normalize(christName);
        FamilyName = string.IsNullOrWhiteSpace(familyName) ? null : Normalize(familyName);
        MiddleName = string.IsNullOrWhiteSpace(middleName) ? null : Normalize(middleName);
        GivenName = !string.IsNullOrWhiteSpace(givenName)
            ? Normalize(givenName)
            : throw new DomainException(DomainErrors.ValidationRequired);
    }

    #endregion

    #region Properties

    /// <summary>
    /// Christ Name
    /// </summary>
    public string? ChristName { get; set; }

    /// <summary>
    /// Family Name
    /// </summary>
    public string? FamilyName { get; }

    /// <summary>
    /// Middle Name
    /// </summary>
    public string? MiddleName { get; }

    /// <summary>
    /// Given Name
    /// </summary>
    public string GivenName { get; }

    #endregion

    #region Methods

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return ChristName;
        yield return FamilyName;
        yield return MiddleName;
        yield return GivenName;
    }

    /// <summary>
    /// String
    /// </summary>
    public override string ToString()
    {
        return string.Join(" ",
            new[] { ChristName, FamilyName, MiddleName, GivenName }
            .Where(x => x != null));
    }

    private static string Normalize(string value)
    {
        return value.Trim();
    }

    #endregion
}