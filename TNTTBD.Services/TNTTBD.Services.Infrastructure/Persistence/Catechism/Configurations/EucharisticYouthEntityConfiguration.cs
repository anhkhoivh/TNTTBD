using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TNTTBD.Services.Domain.ValueObjects;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Configurations;

/// <summary>
/// EucharisticYouth entity configuration
/// </summary>
public class EucharisticYouthEntityConfiguration : IEntityTypeConfiguration<EucharisticYouthEntity>
{
    #region Default Values

    private const string Id = "Id";

    #endregion

    #region Methods

    /// <summary>
    /// Configure method
    /// </summary>
    public void Configure(EntityTypeBuilder<EucharisticYouthEntity> builder)
    {
        builder.ToTable(nameof(EucharisticYouthEntity));
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

        builder.HasMany<CatechismClassEucharisticYouthEntity>()
            .WithOne()
            .HasForeignKey(ce => ce.EucharisticYouthId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany<EucharisticYouthContactEntity>()
            .WithOne()
            .HasForeignKey(ce => ce.EucharisticYouthId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsMany(t => t.Photos, photo =>
        {
            photo.ToTable(nameof(EucharisticYouthEntity.Photos));

            photo.WithOwner()
                 .HasForeignKey(nameof(EucharisticYouthEntity.Id));

            photo.Property<Guid>(Id);
            photo.HasKey(Id);

            photo.Property(p => p.StorageKey)
                 .IsRequired();

            photo.Property(p => p.ContentType)
                 .IsRequired();

            photo.Property(p => p.Purpose)
                 .IsRequired();

            photo.Property(p => p.UploadedAt)
                 .IsRequired();

            photo.Property(p => p.UploadedBy)
                 .IsRequired();

            photo.Property(p => p.Visibility)
                 .IsRequired();
        });

        builder.Navigation(t => t.Photos)
               .UsePropertyAccessMode(PropertyAccessMode.Field);
    }

    #endregion
}