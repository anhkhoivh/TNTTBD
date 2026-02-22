using System.ComponentModel.DataAnnotations;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

public class CatechismClassEucharisticYouthEntity
{
    #region Properties

    /// <summary>
    /// Id
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Catechism Class Id
    /// </summary>
    public Guid CatechismClassId { get; set; }

    /// <summary>
    /// EucharisticYouth Id
    /// </summary>
    public Guid EucharisticYouthId { get; set; }

    #endregion
}