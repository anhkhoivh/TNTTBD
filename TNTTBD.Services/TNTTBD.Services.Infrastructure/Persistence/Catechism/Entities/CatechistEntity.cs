using System.ComponentModel.DataAnnotations;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

/// <summary>
/// Student entity
/// </summary>
public class CatechistEntity
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
    /// Date Of Birth
    /// </summary>
    public DateOnly DateOfBirth { get; set; }

    /// <summary>
    /// Address
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Is male
    /// </summary>
    public bool IsMale { get; set; }

    /// <summary>
    /// Photo collection
    /// </summary>
    public ICollection<PhotoReference> Photos { get; set; } = new List<PhotoReference>();

    /// <summary>
    /// Spiritual Milestone collection
    /// </summary>
    public ICollection<SpiritualMilestoneEntity> SpiritualMilestones { get; set; } = new List<SpiritualMilestoneEntity>();

    /// <summary>
    /// Patron Saints
    /// </summary>
    public ICollection<PatronSaint> PatronSaints { get; set; } = new List<PatronSaint>();

    #endregion
}
