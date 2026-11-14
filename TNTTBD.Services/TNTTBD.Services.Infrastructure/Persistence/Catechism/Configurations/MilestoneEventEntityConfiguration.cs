using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TNTTBD.Services.Domain.Catechism;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Configurations;

/// <summary>
/// Milestone event entity configuration
/// </summary>
public class MilestoneEventEntityConfiguration : IEntityTypeConfiguration<MilestoneEventEntity>
{
    public void Configure(EntityTypeBuilder<MilestoneEventEntity> builder)
    {
        builder.ToTable(nameof(MilestoneEventEntity));
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).IsRequired().HasMaxLength(MilestoneEvent.MaxNameLength);
        builder.Property(e => e.Place).HasMaxLength(MilestoneEvent.MaxPlaceLength);
        builder.Property(e => e.Celebrant).HasMaxLength(MilestoneEvent.MaxCelebrantLength);
        builder.Property(e => e.Note).HasMaxLength(MilestoneEvent.MaxNoteLength);
        builder.Property(e => e.CodeTemplate).HasMaxLength(MilestoneCodeTemplate.MaxTemplateLength);
        builder.HasIndex(e => new { e.Type, e.Date });
    }
}
