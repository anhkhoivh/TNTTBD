using System.ComponentModel.DataAnnotations;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

/// <summary>
/// Student entity
/// </summary>
public class EucharisticYouthEntity
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public EucharisticYouthEntity()
    { }

    #endregion

    #region Properties

    /// <summary>
    /// Id
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Christ Name
    /// </summary>
    public string? ChristName { get; set; }

    /// <summary>
    /// Family Name
    /// </summary>
    public required string FamilyName { get; set; }

    /// <summary>
    /// Middle Name
    /// </summary>
    public string? MiddleName { get; set; }

    /// <summary>
    /// Given Name
    /// </summary>
    public required string GivenName { get; set; }

    /// <summary>
    /// Date Of Birth
    /// </summary>
    public DateOnly DateOfBirth { get; set; }

    /// <summary>
    /// Date of Baptism
    /// </summary>
    public DateOnly DateOfBaptism { get; set; }

    /// <summary>
    /// Address
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// List of Contacts
    /// </summary>
    public ICollection<ContactEntity> Contacts { get; set; } = new List<ContactEntity>();

    /// <summary>
    /// CatechismClassEucharisticYouth join
    /// </summary>
    public ICollection<CatechismClassEucharisticYouthEntity> CatechismClassEucharisticYouths = new List<CatechismClassEucharisticYouthEntity>();

    #endregion
}
