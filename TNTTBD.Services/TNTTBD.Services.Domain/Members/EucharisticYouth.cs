using TNTTBD.Services.Domain.ValueObjects;
using TNTTBD.Services.Models;

namespace TNTTBD.Services.Domain.Members;

/// <summary>
/// Student entity
/// </summary>
public class EucharisticYouth
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public EucharisticYouth()
    { }

    #endregion

    #region Properties

    /// <summary>
    /// Family Name
    /// </summary>
    public FullName Name { get; private set; }
    
    /// <summary>
    /// User
    /// </summary>
    public User User { get; set; }

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

    /// <summary>
    /// Zone
    /// </summary>
    public string Zone { get; set; }

    /// <summary>
    /// List of Contacts
    /// </summary>
    public List<Contact> Contacts { get; set; } = new List<Contact>();

    #endregion
}
