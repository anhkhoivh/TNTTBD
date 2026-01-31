using TNTTBD.Services.Domain.Common;

namespace TNTTBD.Services.Domain.Catechism;

/// <summary>
/// Student entity
/// </summary>
public class CatechismYear : AggregateRoot<Guid>
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public CatechismYear() : base(Guid.Empty)
    {
        Theme = string.Empty;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Start Date
    /// </summary>
    public DateOnly StartDate { get; }

    /// <summary>
    /// End Date
    /// </summary>
    public DateOnly EndDate { get; }

    /// <summary>
    /// Theme
    /// </summary>
    public string Theme { get; }

    /// <summary>
    /// Class in the Catechism Year
    /// </summary>
    public List<CatechismClass> Classes { get; set; } = new List<CatechismClass>();

    #endregion

}
