using System.ComponentModel.DataAnnotations;

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
    /// Christ Name
    /// </summary>
    public required string ChristName { get; set; }

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
