namespace TNTTBD.Services.Domain.Catechism.Members;

/// <summary>
/// Student entity
/// </summary>
public class EucharisticYouth : Member
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public EucharisticYouth() : base()
    {
        Address = string.Empty;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Date Of Birth
    /// </summary>
    public DateOnly DateOfBirth { get; set; }

    /// <summary>
    /// Date of Baptism
    /// </summary>
    public DateOnly DateOfBaptism { get; set; }

    /// <summary>
    /// Address
    /// </summary>
    public string Address { get; set; }

    #endregion
}
