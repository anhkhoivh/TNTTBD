using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TNTTBD.Services.Domain.ValueObjects;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Configurations;

/// <summary>
/// Catechist entity configuration
/// </summary>
public class CatechistEntityConfiguration : IEntityTypeConfiguration<CatechistEntity>
{
    #region Methods

    /// <summary>
    /// Configure method
    /// </summary>
    public void Configure(EntityTypeBuilder<CatechistEntity> builder)
    {
        builder.ToTable(nameof(CatechistEntity));
        builder.HasKey(c => c.Id);

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

        builder.HasMany<CatechismClassCatechistEntity>()
            .WithOne()
            .HasForeignKey(cc => cc.CatechistId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsMany(t => t.Photos, photo =>
        {
            photo.ToTable(nameof(CatechistEntity.Photos));

            photo.WithOwner()
                 .HasForeignKey(nameof(CatechistEntity.Id));

            photo.Property<Guid>("Id");
            photo.HasKey("Id");

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