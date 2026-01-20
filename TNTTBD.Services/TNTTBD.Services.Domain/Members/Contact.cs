using System.ComponentModel.DataAnnotations;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Domain.Members;

/// <summary>
/// Student entity
/// </summary>
public class Contact
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public Contact()
    { }

    #endregion

    #region Properties

    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Relation
    /// </summary>
    public string Relation { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    public FullName Name { get; set; }

    /// <summary>
    /// Phone number
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Eucharistic Youngster that owns the contact
    /// </summary>
    public EucharisticYouth EucharisticYouth { get; set; }

    #endregion

}
