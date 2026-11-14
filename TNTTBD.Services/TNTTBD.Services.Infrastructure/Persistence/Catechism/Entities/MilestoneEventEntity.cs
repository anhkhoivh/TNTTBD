using System.ComponentModel.DataAnnotations;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

/// <summary>
/// Group sacrament event (a Mass where several youths receive the same milestone)
/// </summary>
public class MilestoneEventEntity
{
    [Key]
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public MilestoneType Type { get; set; }

    public DateOnly Date { get; set; }

    public string? Place { get; set; }

    public string? Celebrant { get; set; }

    public string? Note { get; set; }

    public string? CodeTemplate { get; set; }

    public int StartIndex { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Guid? UpdatedBy { get; set; }

    public ICollection<SpiritualMilestoneEntity> Milestones { get; set; } = new List<SpiritualMilestoneEntity>();
}
