using TNTTBD.Services.Domain.ValueObjects;
using TNTTBD.Services.Models;

namespace TNTTBD.Services.Domain.Catechism.Members;

/// <summary>
/// Student entity
/// </summary>
public class Catechist : Member
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public Catechist() : base()
    {
        FullName = new FullName();
    }

    #endregion

    #region Properties

    /// <summary>
    /// Family Name
    /// </summary>
    public FullName FullName { get; private set; }

    #endregion

}
