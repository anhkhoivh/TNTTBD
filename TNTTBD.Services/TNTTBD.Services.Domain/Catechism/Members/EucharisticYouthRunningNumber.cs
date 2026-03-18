using TNTTBD.Services.Domain.Common;

namespace TNTTBD.Services.Domain.Catechism.Members;

/// <summary>
/// Eucharistic Youth running number
/// </summary>
public class EucharisticYouthRunningNumber : DomainEntity<Guid>
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public EucharisticYouthRunningNumber(Guid id, int year, int currentNumber) : base(id)
    {
        Year = year;
        CurrentNumber = currentNumber;
    }

    #endregion

    #region Properties

    public int Year { get; }
    public int CurrentNumber { get; private set; }

    #endregion

    #region Methods

    public void IncreaseNumber()
    {
        CurrentNumber++;
    }

    #endregion
}
