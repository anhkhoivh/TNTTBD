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

    /// <summary>
    /// Constructor with details
    /// </summary>
    public CatechismYear(Guid id, DateOnly startDate, DateOnly endDate, string? theme) : base(id)
    {
        StartDate = startDate;
        EndDate = endDate;
        Theme = theme;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Title format (yyyy-yyyy)
    /// </summary>
    public string Title => $"{StartDate.Year}-{EndDate.Year}";

    /// <summary>
    /// Start Date
    /// </summary>
    public DateOnly StartDate { get; private set; }

    /// <summary>
    /// End Date
    /// </summary>
    public DateOnly EndDate { get; private set; }

    /// <summary>
    /// Theme
    /// </summary>
    public string? Theme { get; private set; }

    #endregion

    #region Methods

    /// <summary>
    /// Update basic info
    /// </summary>
    public void UpdateInfo(DateOnly startDate, DateOnly endDate, string? theme)
    {
        StartDate = startDate;
        EndDate = endDate;
        Theme = theme;
    }

    #endregion
}
