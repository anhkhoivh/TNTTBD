namespace TNTTBD.Services.Domain.Members;

/// <summary>
/// Join model
/// </summary>
public class EucharisticYouthCatechismClass
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public EucharisticYouthCatechismClass()
    { }

    #endregion

    #region Properties

    /// <summary>
    /// Eucharistic Youth Id
    /// </summary>
    public Guid EucharisticYouthId { get; private set; }

    /// <summary>
    /// Catechism Class Id
    /// </summary>
    public Guid CatechismClassId { get; private set; }


    /// <summary>
    /// Scholastic Year Id
    /// </summary>
    public Guid ScholasticYearId { get; private set; }

    #endregion
}
