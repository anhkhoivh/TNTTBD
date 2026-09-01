using System.ComponentModel.DataAnnotations;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

/// <summary>
/// SpiritualMilestoneEntity
/// </summary>
public class SpiritualMilestoneEntity
{
    #region Properties

    /// <summary>
    /// Id
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// EY id
    /// </summary>
    public Guid EucharisticYouthId { get; set; }

    /// <summary>
    /// Youth
    /// </summary>
    public EucharisticYouthEntity EucharisticYouth { get; set; } = null!;

    /// <summary>
    /// Type
    /// </summary>
    public MilestoneType Type { get; set; }

    /// <summary>
    /// Date
    /// </summary>
    public DateOnly? Date { get; set; }

    /// <summary>
    /// Id
    /// </summary>
    public string? DocumentCode { get; set; }

    /// <summary>
    /// Place
    /// </summary>
    public string? Place { get; set; }

    /// <summary>
    /// Note
    /// </summary>
    public string? Note { get; set; }

    #endregion
}
