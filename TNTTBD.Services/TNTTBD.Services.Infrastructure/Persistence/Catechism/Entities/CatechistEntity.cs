using System.ComponentModel.DataAnnotations;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

/// <summary>
/// Student entity
/// </summary>
public class CatechistEntity
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public CatechistEntity()
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
    /// User id
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// User
    /// </summary>
    public UserEntity User { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public ICollection<CatechismClassCatechistEntity> CatechismClassCatechists { get; set; } = new List<CatechismClassCatechistEntity>();

    #endregion

}
