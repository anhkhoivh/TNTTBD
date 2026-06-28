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
    private const string EucharisticYouthIdFk = "EucharisticYouthId";
    private const string EucharisticYouthPatronSaints = "EucharisticYouthPatronSaints";

    #endregion

    #region Methods

    /// <summary>
    /// Configure method
    /// </summary>
    public void Configure(EntityTypeBuilder<EucharisticYouthEntity> builder)
    {
        builder.ToTable(nameof(EucharisticYouthEntity));
        builder.HasKey(e => e.Id);

        builder.Property(y => y.Code)
            .IsRequired();
        builder.HasIndex(y => y.Code)
            .IsUnique();

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

        builder.HasMany(y => y.CatechismClassEucharisticYouths)
            .WithOne(cy => cy.EucharisticYouth)
            .HasForeignKey(ce => ce.EucharisticYouthId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(y => y.EucharisticYouthContacts)
            .WithOne(c => c.EucharisticYouth)
            .HasForeignKey(ce => ce.EucharisticYouthId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Photos)
            .WithOne(p => p.EucharisticYouth)
            .HasForeignKey(p => p.EucharisticYouthId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.SpiritualMilestones)
            .WithOne(m => m.EucharisticYouth)
            .HasForeignKey(m => m.EucharisticYouthId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsMany(e => e.PatronSaints, b =>
        {
            b.WithOwner().HasForeignKey(EucharisticYouthIdFk);

            b.Property<Guid>(Id);
            b.HasKey(Id);

            b.Property(p => p.SaintName)
                .IsRequired();

            b.Property(p => p.Day)
                .IsRequired();

            b.Property(p => p.Month)
                .IsRequired();

            b.ToTable(EucharisticYouthPatronSaints);
        });

        builder.Navigation(t => t.Photos)
               .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasOne(c => c.User)
            .WithOne(u => u.EucharisticYouth)
            .HasForeignKey<EucharisticYouthEntity>(c => c.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }

    #endregion
}