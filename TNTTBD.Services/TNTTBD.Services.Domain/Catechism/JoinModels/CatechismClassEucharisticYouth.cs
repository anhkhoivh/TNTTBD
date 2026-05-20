using TNTTBD.Services.Domain.Common;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Domain.Catechism.JoinModels;

/// summary>
/// Student entity
/// </summary>
public class CatechismClassEucharisticYouth : DomainEntity<Guid>
{
    #region Constructors

    /// <summary>
    /// Default
    /// </summary>
    public CatechismClassEucharisticYouth() : base(Guid.Empty) { }

    /// <summary>
    /// Constructor from details
    /// </summary>
    public CatechismClassEucharisticYouth(
        Guid id, 
        Guid eucharisticYouthId,
        Guid catechismClassId,
        DateTimeOffset enrolledAt,
        DateTimeOffset? leftAt,
        EnrollmentStatus status,
        string? note) : base(id)
    {
        EucharisticYouthId = eucharisticYouthId;
        CatechismClassId = catechismClassId;
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
    public Guid EucharisticYouthId { get; private set; }

    /// <summary>
    /// Date in
    /// </summary>
    public DateTimeOffset EnrolledAt { get; init; }

    /// <summary>
    /// Date out
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

}
