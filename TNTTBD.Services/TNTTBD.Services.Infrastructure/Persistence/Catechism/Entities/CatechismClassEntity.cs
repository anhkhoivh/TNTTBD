using System.ComponentModel.DataAnnotations;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

/// <summary>
/// Student entity
/// </summary>
public class CatechismClassEntity
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public CatechismClassEntity()
    { }

    #endregion

    #region Properties

    /// <summary>
    /// Id
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Class Name
    /// </summary>
    public string ClassName { get; set; }

    /// <summary>
    /// Catechism Year Id
    /// </summary>
    public Guid CatechismYearId { get; set; }

    /// <summary>
    /// Catechism Year
    /// </summary>
    public CatechismYearEntity CatechismYear { get; set; }

    /// <summary>
    /// Assistant Catechist List
    /// </summary>
    public ICollection<CatechistEntity> ClassCatechists { get; set; } = new List<CatechistEntity>();
    
    /// <summary>
    /// 
    /// </summary>
    public ICollection<EucharisticYouthEntity> EucharisticYouths { get; set; } = new List<EucharisticYouthEntity>();

    /// <summary>
    /// Catechism Session List
    /// </summary>
    public ICollection<CatechismSessionEntity> Sessions { get; set; }  = new List<CatechismSessionEntity>();

    #endregion

}
