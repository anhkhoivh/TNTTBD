using TNTTBD.Services.Domain.Common;

namespace TNTTBD.Services.Domain.ValueObjects;

/// <summary>
/// Patron saint of member
/// </summary>
public class PatronSaint : ValueObject
{
    #region Constructors

    public PatronSaint(string name, int day, int month)
    {
        SaintName = name;
        Day = day;
        Month = month;
    }

    #endregion

    #region Properties

    public string SaintName { get; }
    public int Day { get; }
    public int Month { get; }

    #endregion

    #region Methods

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return SaintName;
        yield return Day;
        yield return Month;
    }

    #endregion
}