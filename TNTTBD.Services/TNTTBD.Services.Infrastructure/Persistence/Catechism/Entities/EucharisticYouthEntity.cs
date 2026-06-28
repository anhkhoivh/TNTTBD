using System.ComponentModel.DataAnnotations;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

/// <summary>
/// Student entity
/// </summary>
public class EucharisticYouthEntity
{
    #region Properties

    /// <summary>
    /// Id
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Code
    /// </summary>
    public string Code { get; set; } = null!;

    /// <summary>
    /// Full Name
    /// </summary>
    public FullName FullName { get; set; } = null!;

    /// <summary>
    /// Date Of Birth
    /// </summary>
    public DateOnly? DateOfBirth { get; set; }

    /// <summary>
    /// Address
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Phone Number
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Is male
    /// </summary>
    public bool IsMale { get; set; }

    /// <summary>
    /// Photo collection
    /// </summary>
    public ICollection<EucharisticYouthPhotoEntity> Photos { get; set; } = new List<EucharisticYouthPhotoEntity>();

    /// <summary>
    /// Spiritual Milestone collection
    /// </summary>
    public ICollection<SpiritualMilestoneEntity> SpiritualMilestones { get; set; } = new List<SpiritualMilestoneEntity>();

    /// <summary>
    /// Patron Saints
    /// </summary>
    public ICollection<PatronSaint> PatronSaints { get; set; } = new List<PatronSaint>();

    /// <summary>
    /// Youth List
    /// </summary>
    public ICollection<CatechismClassEucharisticYouthEntity> CatechismClassEucharisticYouths { get; set; } = new List<CatechismClassEucharisticYouthEntity>();

    
    /// <summary>
    /// Contacts
    /// </summary>
    public ICollection<EucharisticYouthContactEntity> EucharisticYouthContacts { get; set; } = new List<EucharisticYouthContactEntity>();

    /// <summary>
    /// Id
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// User
    /// </summary>
    public UserEntity? User { get; set; }

    #endregion
}
