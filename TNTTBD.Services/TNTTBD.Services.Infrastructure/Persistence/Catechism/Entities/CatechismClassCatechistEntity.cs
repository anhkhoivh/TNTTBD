namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

public class CatechismClassCatechistEntity
{
    #region Properties

    /// <summary>
    /// Catechism Class Id
    /// </summary>
    public Guid CatechismClassId { get; set; }

    /// <summary>
    /// Catechism Class
    /// </summary>
    public CatechismClassEntity CatechismClass { get; set; } = null!;

    /// <summary>
    /// Catechist Id
    /// </summary>
    public Guid CatechistId { get; set; }

    /// <summary>
    /// Catechist
    /// </summary>
    public CatechistEntity Catechist { get; set; } = null!;

    /// <summary>
    /// Is catechist primary
    /// </summary>
    public bool IsPrimary { get; set; }

    #endregion
}