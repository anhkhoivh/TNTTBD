namespace TNTTBD.Services.Domain.Catechism;

/// <summary>
/// Session entity
/// </summary>
public class CatechismSession
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public CatechismSession()
    { }

    #endregion

    #region Properties

    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Date
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Theme
    /// </summary>
    public string Theme { get; set; }

    /// <summary>
    /// Catechism Class of the Session
    /// </summary>
    public CatechismClass Class { get; set; }

    #endregion

}
