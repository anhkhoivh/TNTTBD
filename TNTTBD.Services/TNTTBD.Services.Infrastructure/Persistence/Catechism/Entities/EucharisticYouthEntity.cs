using System.ComponentModel.DataAnnotations;
using TNTTBD.Services.Domain.ValueObjects;

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
    /// Full Name
    /// </summary>
    public FullName FullName { get; set; } = null!;

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
