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

        builder.HasMany(ce => ce.EucharisticYouthAttendances)
            .WithOne(a => a.CatechismClassEucharisticYouth)
            .HasForeignKey(c => c.CatechismClassEucharisticYouthId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(ce => ce.ExamResults)
            .WithOne(e => e.CatechismClassEucharisticYouth)
            .HasForeignKey(c => c.CatechismClassEucharisticYouthId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    #endregion
}