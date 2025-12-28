using System.ComponentModel.DataAnnotations;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

/// <summary>
/// Session entity
/// </summary>
public class CatechismSessionEntity
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public CatechismSessionEntity()
    { }

    #endregion

    #region Properties

    /// <summary>
    /// Id
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Date
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Title
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Class Id
    /// </summary>
    public Guid ClassId { get; set; }

    /// <summary>
    /// Catechism Class of the Session
    /// </summary>
    public CatechismClassEntity Class { get; set; }

    public ICollection<CatechismSessionAttendanceEntity> EucharisticYouthAttendances { get; set; } = new List<CatechismSessionAttendanceEntity>();

    #endregion

}
