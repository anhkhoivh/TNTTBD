using TNTTBD.Services.Domain.Common;

namespace TNTTBD.Services.Domain.Catechism;

/// summary>
/// Student entity
/// </summary>
public class CatechismClass : AggregateRoot<Guid>
{
    #region Constructors

    /// <summary>
    /// Default
    /// </summary>
    public CatechismClass() : base(Guid.Empty)
    {
        ClassName = string.Empty;
    }

    /// <summary>
    /// Constructor from parameters
    /// </summary>
    /// <param name="id"></param>
    /// <param name="name"></param>
    public CatechismClass(Guid id, Guid catechismYearId, string className)
        : base(id)
    {
        ClassName = className;
        CatechismYearId = catechismYearId;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Class Name
    /// </summary>
    public string ClassName { get; private set; }

    /// <summary>
    /// Catechism Year
    /// </summary>
    public Guid CatechismYearId { get; private set; }

    #endregion

    #region Methods

    /// <summary>
    /// Update class info
    /// </summary>
    /// <param name="name"></param>
    public void UpdateInfo(string name)
    {
        ClassName = name;
    }

    #endregion
}
