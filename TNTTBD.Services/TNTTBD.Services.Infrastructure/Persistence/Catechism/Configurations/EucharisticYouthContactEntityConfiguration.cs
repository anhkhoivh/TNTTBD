using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Configurations;

/// <summary>
/// Catechist entity configuration
/// </summary>
public class EucharisticYouthContactEntityConfiguration : IEntityTypeConfiguration<EucharisticYouthContactEntity>
{
    #region Methods

    /// <summary>
    /// Configure method
    /// </summary>
    public void Configure(EntityTypeBuilder<EucharisticYouthContactEntity> builder)
    {
        builder.ToTable(nameof(EucharisticYouthContactEntity));
        builder.HasKey(c => new { c.ContactId, c.EucharisticYouthId });

        builder.HasIndex(x => new
        {
            x.ContactId,
            x.EucharisticYouthId,
        }).IsUnique();
    }

    #endregion
}