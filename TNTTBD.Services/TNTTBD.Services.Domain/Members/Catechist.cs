using System.ComponentModel.DataAnnotations;
using TNTTBD.Services.Domain.ValueObjects;
using TNTTBD.Services.Models;

namespace TNTTBD.Services.Domain.Members;

/// <summary>
/// Student entity
/// </summary>
public class Catechist
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public Catechist()
    { }

    #endregion

    #region Properties

    /// <summary>
    /// Family Name
    /// </summary>
    public FullName FullName { get; private set; }
    
    /// <summary>
    /// User
    /// </summary>
    public User User { get; set; }

    #endregion

}
