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
    /// Christ Name
    /// </summary>
    public string ChristName { get; set; }

    /// <summary>
    /// Family Name
    /// </summary>
    public FullName Name { get; private set; }
    
    /// <summary>
    /// User
    /// </summary>
    public User User { get; set; }
    
    /// <summary>
    /// Full Name
    /// </summary>
    public string FullName => $"{ChristName} {Name.Display()}";

    #endregion

}
