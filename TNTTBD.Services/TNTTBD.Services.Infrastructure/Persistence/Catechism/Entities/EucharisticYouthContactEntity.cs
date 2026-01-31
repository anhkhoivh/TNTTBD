using System.ComponentModel.DataAnnotations;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

/// <summary>
/// Student entity
/// </summary>
public class EucharisticYouthContactEntity
{
    #region Properties

    /// <summary>
    /// Id
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Eucharistic Younth Id
    /// </summary>
    public Guid EucharisticYouthId { get; set; }

    /// <summary>
    /// Contact Id
    /// </summary>
    public Guid ContactId { get; set; }

    /// <summary>
    /// Contact Relation Type
    /// </summary>
    public ContactRelationType ContactRelationType { get; set; }

    #endregion

}
