using TNTTBD.Services.Domain.Common;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Domain.Catechism.Members;

/// <summary>
/// Spiritual Milestone record
/// </summary>
public class SpiritualMilestone : DomainEntity<Guid>
{
    #region Constructors

    public SpiritualMilestone(
        Guid id, 
        MilestoneType type, 
        DateOnly? date, 
        string? documentCode,
        string? place = null, 
        string? note = null) : base(id)
    {
        Type = type;
        Date = date;
        Place = place;
        Note = note;
        DocumentCode = documentCode;
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
    public DateOnly? Date { get; private set; }

    /// <summary>
    /// Id
    /// </summary>
    public string? DocumentCode { get; private set; }

    /// <summary>
    /// Place
    /// </summary>
    public string? Place { get; private set; }

    /// <summary>
    /// Note
    /// </summary>
    public string? Note { get; private set; }

    #endregion

    #region Methods

    /// <summary>
    /// Update
    /// </summary>
    public void Update(DateOnly? date, string? documentCode, string? place, string? note)
    {
        Date = date;
        DocumentCode = documentCode;
        Place = place;
        Note = note;
    }

    #endregion
}