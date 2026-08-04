using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Configurations;

/// <summary>
/// Catechist entity configuration
/// </summary>
public class EucharisticYouthPhotoEntityConfiguration : IEntityTypeConfiguration<EucharisticYouthPhotoEntity>
{
    #region Methods

    /// <summary>
    /// Configure method
    /// </summary>
    public void Configure(EntityTypeBuilder<EucharisticYouthPhotoEntity> builder)
    {
        builder.ToTable(nameof(EucharisticYouthPhotoEntity));
        builder.HasKey(c => c.Id);
    }

    #endregion
}