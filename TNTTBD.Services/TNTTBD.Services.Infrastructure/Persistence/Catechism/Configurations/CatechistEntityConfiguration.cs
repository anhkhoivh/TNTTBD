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
    #region Default Values

    private const string Id = "Id";
    private const string CatechistIdFk = "CatechistId";
    private const string CatechistPatronSaints = "CatechistPatronSaints";

    #endregion

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

        builder.HasMany(c => c.CatechismClassCatechists)
            .WithOne(cc => cc.Catechist)
            .HasForeignKey(cc => cc.CatechistId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Photos)
            .WithOne(p => p.Catechist)
            .HasForeignKey(p => p.CatechistId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsMany(e => e.PatronSaints, b =>
        {
            b.WithOwner().HasForeignKey(CatechistIdFk);

            b.Property<Guid>(Id);
            b.HasKey(Id);

            b.Property(p => p.SaintName)
                .IsRequired();

            b.Property(p => p.Day)
                .IsRequired();

            b.Property(p => p.Month)
                .IsRequired();

            b.ToTable(CatechistPatronSaints);
        });

        builder.HasOne(c => c.User)
            .WithOne(u => u.Catechist)
            .HasForeignKey<CatechistEntity>(c => c.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Navigation(t => t.Photos)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }

    #endregion
}