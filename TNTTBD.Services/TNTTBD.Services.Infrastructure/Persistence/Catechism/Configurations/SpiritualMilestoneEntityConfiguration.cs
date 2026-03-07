using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Configurations;

/// <summary>
/// Catechist entity configuration
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

        builder.HasOne<EucharisticYouthEntity>()
            .WithMany(y => y.SpiritualMilestones)
            .HasForeignKey(c => c.EucharisticYouthId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    #endregion
}