using TNTTBD.Services.Domain.Common;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Domain.Catechism.JoinModels;

/// summary>
/// Student entity
/// </summary>
public class EucharisticYouthContact : DomainEntity<Guid>
{
    #region Constructors

    /// <summary>
    /// Default
    /// </summary>
    public EucharisticYouthContact() : base(Guid.Empty) { }

    /// <summary>
    /// Constructor from details
    /// </summary>
    public EucharisticYouthContact(
        Guid id, 
        Guid eucharisticYouthId,
        Guid contactId) : base(id)
    {
        EucharisticYouthId = eucharisticYouthId;
        ContactId = contactId;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Catechism Class Id
    /// </summary>
    public Guid ContactId { get; private set; }

    /// <summary>
    /// Catechist Id
    /// </summary>
    public Guid EucharisticYouthId { get; private set; }

    /// <summary>
    /// Contact Relation Type
    /// </summary>
    public ContactRelationType ContactRelationType { get; private set; }

    #endregion

}
