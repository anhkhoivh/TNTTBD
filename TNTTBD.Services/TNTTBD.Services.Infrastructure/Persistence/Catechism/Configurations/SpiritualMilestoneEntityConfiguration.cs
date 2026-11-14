using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Configurations;

/// <summary>
/// Spiritual milestone entity configuration
/// </summary>
public class SpiritualMilestoneEntityConfiguration : IEntityTypeConfiguration<SpiritualMilestoneEntity>
{
    #region Methods

    /// <summary>
    /// Configure method
    /// </summary>
    public void Configure(EntityTypeBuilder<SpiritualMilestoneEntity> builder)
    {
        builder.ToTable(nameof(SpiritualMilestoneEntity));
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Celebrant).HasMaxLength(200);

        builder.HasOne(c => c.SponsorContact)
            .WithMany()
            .HasForeignKey(c => c.SponsorContactId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(c => new { c.EucharisticYouthId, c.Type });

        builder.HasOne(c => c.MilestoneEvent)
            .WithMany(e => e.Milestones)
            .HasForeignKey(c => c.MilestoneEventId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    #endregion
}
