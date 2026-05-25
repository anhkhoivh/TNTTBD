using TNTTBD.Services.Domain.Common;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Domain.Catechism.JoinModels;

/// summary>
/// Student entity
/// </summary>
public class CatechismClassCatechist : DomainEntity<Guid>
{
    #region Constructors

    /// <summary>
    /// Default
    /// </summary>
    public CatechismClassCatechist() : base(Guid.Empty) { }

    /// <summary>
    /// Constructor from details
    /// </summary>
    public CatechismClassCatechist(
        Guid id,
        Guid catechistId, 
        Guid catechismClassId, 
        CatechismClassCatechistRole role,
        DateTimeOffset enrolledAt,
        DateTimeOffset? leftAt,
        EnrollmentStatus status,
        string? note) : base(id)
    {
        CatechistId = catechistId;
        CatechismClassId = catechismClassId;
        Role = role;
        EnrolledAt = enrolledAt;
        LeftAt = leftAt;
        Status = status;
        Note = note;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Catechism Class Id
    /// </summary>
    public Guid CatechismClassId { get; private set; }

    /// <summary>
    /// Catechist Id
    /// </summary>
    public Guid CatechistId { get; private set; }

    /// <summary>
    /// Catechist role
    /// </summary>
    public CatechismClassCatechistRole Role { get; private set; }

    /// <summary>
    /// Date enrolled
    /// </summary>
    public DateTimeOffset EnrolledAt { get; init; }

    /// <summary>
    /// Date left
    /// </summary>
    public DateTimeOffset? LeftAt { get; private set; }

    /// <summary>
    /// Status
    /// </summary>
    public EnrollmentStatus Status { get; private set; }

    /// <summary>
    /// Note
    /// </summary>
    public string? Note { get; private set; }

    #endregion

    #region Methods

    public void ChangeRole(CatechismClassCatechistRole newRole)
    {
        Role = newRole;
    }

    public void EndEnrollment(EnrollmentStatus status, string? note = null)
    {
        Status = status;
        LeftAt = DateTimeOffset.UtcNow;
        Note = note;
    }

    #endregion
}
