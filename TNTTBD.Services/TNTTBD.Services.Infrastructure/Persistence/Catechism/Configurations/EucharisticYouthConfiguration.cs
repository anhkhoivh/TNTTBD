using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TNTTBD.Services.Domain.Members;
using TNTTBD.Services.Domain.ValueObjects;
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

        builder.OwnsOne(x => x.FullName, name =>
        {
            name.Property(n => n.ChristName)
                .HasColumnName(nameof(FullName.ChristName));

            name.Property(n => n.FamilyName)
                .HasColumnName(nameof(FullName.FamilyName));

            name.Property(n => n.MiddleName)
                .HasColumnName(nameof(FullName.MiddleName));

            name.Property(n => n.GivenName)
                .HasColumnName(nameof(FullName.GivenName))
                .IsRequired();
        });

        builder.HasMany(e => e.CatechismClassEucharisticYouths)
            .WithOne(ce => ce.EucharisticYouth)
            .HasForeignKey(ce => ce.EucharisticYouthId);
    }

    #endregion
}