using System.ComponentModel.DataAnnotations;
using TNTTBD.Services.Domain.Enums;
using TNTTBD.Services.Domain.ValueObjects;

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
    /// Full Name
    /// </summary>
    public FullName FullName { get; set; } = null!;

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
