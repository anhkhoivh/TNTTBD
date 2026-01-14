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
    /// Catechist List
    /// </summary>
    public ICollection<CatechismClassCatechistEntity> CatechismClassCatechists { get; set; } = new List<CatechismClassCatechistEntity>();
    
    /// <summary>
    /// 
    /// </summary>
    public ICollection<CatechismClassEucharisticYouthEntity> CatechismClassEucharisticYouths { get; set; } = new List<CatechismClassEucharisticYouthEntity>();

    /// <summary>
    /// Catechism Session List
    /// </summary>
    public ICollection<CatechismSessionEntity> Sessions { get; set; }  = new List<CatechismSessionEntity>();

    /// <summary>
    /// Exam List
    /// </summary>
    public ICollection<ExamEntity> Exams { get; set; } = new List<ExamEntity>();

    #endregion

}
