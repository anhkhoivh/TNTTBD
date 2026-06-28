using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Configurations;

/// <summary>
/// Catechist entity configuration
/// </summary>
public class CatechismClassEucharisticYouthEntityConfiguration : IEntityTypeConfiguration<CatechismClassEucharisticYouthEntity>
{
    #region Methods

    /// <summary>
    /// Configure method
    /// </summary>
    public void Configure(EntityTypeBuilder<CatechismClassEucharisticYouthEntity> builder)
    {
        builder.ToTable(nameof(CatechismClassEucharisticYouthEntity));
        builder.HasKey(c => c.Id);

        builder.HasIndex(x => new
        {
            x.CatechismClassId,
            x.EucharisticYouthId
        }).IsUnique();

        builder.HasOne(ce => ce.CatechismClass)
            .WithMany(c => c.CatechismClassEucharisticYouths)
            .HasForeignKey(c => c.CatechismClassId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ce => ce.EucharisticYouth)
            .WithMany(y => y.CatechismClassEucharisticYouths)
            .HasForeignKey(ey => ey.EucharisticYouthId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    #endregion
}