using System.ComponentModel.DataAnnotations;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

/// <summary>
/// Student entity
/// </summary>
public class ContactEntity
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public ContactEntity()
    { }

    #endregion

    #region Properties

    /// <summary>
    /// Id
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Relation
    /// </summary>
    public string Relation { get; set; }

    /// <summary>
    /// Relation
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Phone number
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Eucharistic Youngster Id
    /// </summary>
    public Guid EucharisticYouthId { get; set; }

    /// <summary>
    /// Eucharistic Youngster that owns the contact
    /// </summary>
    public EucharisticYouthEntity EucharisticYouth { get; set; }

    #endregion

}
