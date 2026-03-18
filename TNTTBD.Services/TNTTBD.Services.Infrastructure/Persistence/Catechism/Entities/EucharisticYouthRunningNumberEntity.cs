using System.ComponentModel.DataAnnotations;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

/// <summary>
/// Eucharistic Youth running number entity
/// </summary>
public class EucharisticYouthRunningNumberEntity
{
    #region Properties

    /// <summary>
    /// Id
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    public int Year { get; set; }
    public int CurrentNumber { get; set; }


    #endregion
}
