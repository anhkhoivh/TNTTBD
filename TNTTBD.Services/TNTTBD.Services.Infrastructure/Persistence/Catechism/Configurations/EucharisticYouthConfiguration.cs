using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TNTTBD.Services.Domain.Members;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Configurations;

/// <summary>
/// EucharisticYouth entity configuration
/// </summary>
public class EucharisticYouthConfiguration : IEntityTypeConfiguration<EucharisticYouthEntity>
{
    #region Methods

    /// <summary>
    /// Configure method
    /// </summary>
    public void Configure(EntityTypeBuilder<EucharisticYouthEntity> builder)
    {
        builder.ToTable(nameof(EucharisticYouth));
        builder.HasKey(e => e.Id);

        builder.HasMany(e => e.CatechismClassEucharisticYouths)
            .WithOne(ce => ce.EucharisticYouth)
            .HasForeignKey(ce => ce.EucharisticYouthId);
    }

    #endregion
}