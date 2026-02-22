using TNTTBD.Services.Domain.Common;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Domain.Catechism.JoinModels;

/// summary>
/// Student entity
/// </summary>
public class CatechismClassCatechist : DomainEntity<Guid>
{
    #region Constructors

    /// <summary>
    /// Default
    /// </summary>
    public CatechismClassCatechist() : base(Guid.Empty) { }

    /// <summary>
    /// Constructor from details
    /// </summary>
    public CatechismClassCatechist(
        Guid id,
        Guid catechistId, 
        Guid catechismClassId, 
        CatechismClassCatechistRole role) : base(id)
    {
        CatechistId = catechistId;
        CatechismClassId = catechismClassId;
        Role = role;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Catechism Class Id
    /// </summary>
    public Guid CatechismClassId { get; private set; }

    /// <summary>
    /// Catechist Id
    /// </summary>
    public Guid CatechistId { get; private set; }

    /// <summary>
    /// Catechist role
    /// </summary>
    public CatechismClassCatechistRole Role { get; private set; }

    #endregion

    #region Methods

    public void ChangeRole(CatechismClassCatechistRole newRole)
    {
        Role = newRole;
    }

    #endregion
}
