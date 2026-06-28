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
    public string ClassName { get; set; } = null!;

    /// <summary>
    /// <summary>
    /// Class Code
    /// </summary>
    public string ClassCode { get; set; } = null!;

    /// <summary>
    /// Catechism Year Id
    /// </summary>
    public Guid CatechismYearId { get; set; }

    /// <summary>
    /// Catechism Year
    /// </summary>
    public CatechismYearEntity CatechismYear { get; set; } = null!;

    /// <summary>
    /// Catechism Session List
    /// </summary>
    public ICollection<CatechismSessionEntity> CatechismSessions { get; set; }  = new List<CatechismSessionEntity>();

    /// <summary>
    /// Exam List
    /// </summary>
    public ICollection<ExamEntity> Exams { get; set; } = new List<ExamEntity>();

    /// <summary>
    /// Youth List
    /// </summary>
    public ICollection<CatechismClassEucharisticYouthEntity> CatechismClassEucharisticYouths { get; set; } = new List<CatechismClassEucharisticYouthEntity>();

    /// <summary>
    /// Catechist List
    /// </summary>
    public ICollection<CatechismClassCatechistEntity> CatechismClassCatechists { get; set; } = new List<CatechismClassCatechistEntity>();

    #endregion
}
