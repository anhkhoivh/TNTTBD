namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

public class CatechismClassEucharisticYouthEntity
{
    #region Properties

    /// <summary>
    /// Catechism Class Id
    /// </summary>
    public Guid CatechismClassId { get; set; }

    /// <summary>
    /// Catechism Class
    /// </summary>
    public CatechismClassEntity CatechismClass { get; set; } = null!;

    /// <summary>
    /// EucharisticYouth Id
    /// </summary>
    public Guid EucharisticYouthId { get; set; }

    /// <summary>
    /// Eucharistic Youth
    /// </summary>
    public EucharisticYouthEntity EucharisticYouth { get; set; } = null!;

    #endregion
}