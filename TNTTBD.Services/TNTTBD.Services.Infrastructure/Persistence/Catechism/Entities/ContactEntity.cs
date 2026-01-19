using System.ComponentModel.DataAnnotations;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

/// <summary>
/// Student entity
/// </summary>
public class ContactEntity
{
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
    public string? FamilyName { get; set; }

    /// <summary>
    /// Middle Name
    /// </summary>
    public string? MiddleName { get; set; }

    /// <summary>
    /// Given Name
    /// </summary>
    public required string GivenName { get; set; }

    /// <summary>
    /// Phone number
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Email
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// User id
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// User entity
    /// </summary>
    public UserEntity? User { get; set; }

    /// <summary>
    /// EucharisticYouthContacts
    /// </summary>
    public ICollection<EucharisticYouthContactEntity> EucharisticYouthContacts { get; set; } = new List<EucharisticYouthContactEntity>();

    #endregion

}
