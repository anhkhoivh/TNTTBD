using System.ComponentModel.DataAnnotations;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

public class CatechismClassCatechistEntity
{
    #region Properties

    /// <summary>
    /// Id
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Catechism Class Id
    /// </summary>
    public Guid CatechismClassId { get; set; }

    /// <summary>
    /// Catechism Class entity
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
    /// Catechist role
    /// </summary>
    public CatechismClassCatechistRole Role { get; set; }

    /// <summary>
    /// Date enrolled
    /// </summary>
    public DateTimeOffset EnrolledAt { get; set; }

    /// <summary>
    /// Date left
    /// </summary>
    public DateTimeOffset? LeftAt { get; set; }

    /// <summary>
    /// Status
    /// </summary>
    public EnrollmentStatus Status { get; set; }

    /// <summary>
    /// Note
    /// </summary>
    public string? Note { get; set; }

    #endregion
}