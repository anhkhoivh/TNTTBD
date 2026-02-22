using TNTTBD.Services.Domain.Common;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Domain.ValueObjects;

/// <summary>
/// Spiritual Milestone record
/// </summary>
public class SpiritualMilestone : ValueObject
{
    #region Constructors

    public SpiritualMilestone(MilestoneType type, DateOnly date, string? place = null, string? note = null)
    {
        Type = type;
        Date = date;
        Place = place;
        Note = note;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Type
    /// </summary>
    public MilestoneType Type { get; }

    /// <summary>
    /// Date
    /// </summary>
    public DateOnly Date { get; }

    /// <summary>
    /// Id
    /// </summary>
    public string? DocumentCode { get; }

    /// <summary>
    /// Place
    /// </summary>
    public string? Place { get; }

    /// <summary>
    /// Note
    /// </summary>
    public string? Note { get; }

    #endregion

    #region Methods

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Type;
        yield return Date;
        yield return DocumentCode ?? string.Empty;
        yield return Place ?? string.Empty;
        yield return Note ?? string.Empty;
    }

    #endregion
}