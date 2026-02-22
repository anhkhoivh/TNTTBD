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

        builder.HasOne<CatechismClassEntity>()
            .WithMany()
            .HasForeignKey(c => c.CatechismClassId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<EucharisticYouthEntity>()
            .WithMany()
            .HasForeignKey(ey => ey.CatechismClassId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    #endregion
}