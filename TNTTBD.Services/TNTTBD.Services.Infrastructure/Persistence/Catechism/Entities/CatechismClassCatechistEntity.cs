using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

public class CatechismClassCatechistEntity
{
    #region Properties

    /// <summary>
    /// Catechism Class Id
    /// </summary>
    public Guid CatechismClassId { get; set; }

    /// <summary>
    /// Catechist Id
    /// </summary>
    public Guid CatechistId { get; set; }

    /// <summary>
    /// Catechist role
    /// </summary>
    public CatechismClassCatechistRole Role { get; set; }

    #endregion
}