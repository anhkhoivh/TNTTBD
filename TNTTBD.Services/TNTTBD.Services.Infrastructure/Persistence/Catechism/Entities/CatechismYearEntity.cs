using System.ComponentModel.DataAnnotations;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

/// <summary>
/// Student entity
/// </summary>
public class CatechismYearEntity
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public CatechismYearEntity()
    { }

    #endregion

    #region Properties

    /// <summary>
    /// Id
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Start Date
    /// </summary>
    public DateOnly StartDate { get; set; }

    /// <summary>
    /// End Date
    /// </summary>
    public DateOnly EndDate { get; set; }

    /// <summary>
    /// Theme
    /// </summary>
    public string? Theme { get; set; }

    /// <summary>
    /// Class in the Catechism Year
    /// </summary>
    public ICollection<CatechismClassEntity> CatechismClasses { get; set; } = new List<CatechismClassEntity>();

    #endregion

}
