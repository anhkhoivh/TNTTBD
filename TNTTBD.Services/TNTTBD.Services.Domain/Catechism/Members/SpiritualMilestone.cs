using TNTTBD.Services.Domain.Common;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Domain.Catechism.Members;

/// <summary>
/// Spiritual Milestone record
/// </summary>
public class SpiritualMilestone : DomainEntity<Guid>
{
    public const int MaxCelebrantLength = 200;

    #region Constructors

    public SpiritualMilestone(
        Guid id,
        MilestoneType type,
        DateOnly? date,
        string? documentCode,
        string? place = null,
        string? note = null,
        Guid? sponsorContactId = null,
        string? celebrant = null,
        Guid? milestoneEventId = null) : base(id)
    {
        Type = type;
        Date = date;
        Place = place;
        Note = note;
        DocumentCode = NormalizeCode(documentCode);
        SponsorContactId = NormalizeSponsor(type, sponsorContactId);
        Celebrant = NormalizeText(celebrant, MaxCelebrantLength);
        MilestoneEventId = milestoneEventId;
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
    /// Document / certificate code
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

    /// <summary>
    /// Godfather / Godmother contact (Baptism and Confirmation only)
    /// </summary>
    public Guid? SponsorContactId { get; private set; }

    /// <summary>
    /// Bishop / priest who celebrated the sacrament
    /// </summary>
    public string? Celebrant { get; private set; }

    /// <summary>
    /// Optional group sacrament event this milestone was received in
    /// </summary>
    public Guid? MilestoneEventId { get; private set; }

    /// <summary>
    /// Whether this milestone type supports a sponsor
    /// </summary>
    public static bool SupportsSponsor(MilestoneType type) =>
        type == MilestoneType.Baptism || type == MilestoneType.Confirmation;

    #endregion

    #region Methods

    /// <summary>
    /// Update
    /// </summary>
    public void Update(DateOnly? date, string? documentCode, string? place, string? note, Guid? sponsorContactId, string? celebrant)
    {
        Date = date;
        DocumentCode = NormalizeCode(documentCode);
        Place = place;
        Note = note;
        SponsorContactId = NormalizeSponsor(Type, sponsorContactId);
        Celebrant = NormalizeText(celebrant, MaxCelebrantLength);
    }

    /// <summary>
    /// Link to a group sacrament event
    /// </summary>
    public void LinkToEvent(Guid milestoneEventId)
    {
        MilestoneEventId = milestoneEventId;
    }

    /// <summary>
    /// Unlink from its event (keeps the milestone)
    /// </summary>
    public void UnlinkFromEvent()
    {
        MilestoneEventId = null;
    }

    /// <summary>
    /// Normalize a document code (trim, empty to null)
    /// </summary>
    public static string? NormalizeCode(string? code) =>
        string.IsNullOrWhiteSpace(code) ? null : code.Trim();

    private static Guid? NormalizeSponsor(MilestoneType type, Guid? sponsorContactId) =>
        SupportsSponsor(type) && sponsorContactId != Guid.Empty ? sponsorContactId : null;

    private static string? NormalizeText(string? value, int max)
    {
        if (string.IsNullOrWhiteSpace(value)) return null;
        string trimmed = value.Trim();
        return trimmed.Length > max ? trimmed[..max] : trimmed;
    }

    #endregion
}
