using TNTTBD.Services.Domain.Common;

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
        Guid catechismClassId) : base(id)
    {
        EucharisticYouthId = eucharisticYouthId;
        CatechismClassId = catechismClassId;
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

    #endregion

}
