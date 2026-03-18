using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Configurations;

/// <summary>
/// EucharisticYouthRunningNumber entity configuration
/// </summary>
public class EucharisticYouthRunningNumberConfiguration : IEntityTypeConfiguration<EucharisticYouthRunningNumberEntity>
{
    #region Methods

    /// <summary>
    /// Configure method
    /// </summary>
    public void Configure(EntityTypeBuilder<EucharisticYouthRunningNumberEntity> builder)
    {
        builder.ToTable(nameof(EucharisticYouthRunningNumberEntity));
        builder.HasKey(c => c.Id);
    }

    #endregion
}