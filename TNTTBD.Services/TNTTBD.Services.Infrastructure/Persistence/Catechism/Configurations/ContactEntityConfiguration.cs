using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TNTTBD.Services.Domain.ValueObjects;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Configurations;

/// <summary>
/// Catechist entity configuration
/// </summary>
public class ContactEntityConfiguration : IEntityTypeConfiguration<ContactEntity>
{
    #region Methods

    /// <summary>
    /// Configure method
    /// </summary>
    public void Configure(EntityTypeBuilder<ContactEntity> builder)
    {
        builder.ToTable(nameof(ContactEntity));
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

        builder.HasMany(y => y.EucharisticYouthContacts)
            .WithOne(c => c.Contact)
            .HasForeignKey(ce => ce.ContactId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(c => c.User)
            .WithOne(u => u.Contact)
            .HasForeignKey<ContactEntity>(c => c.UserId)
            .OnDelete(DeleteBehavior.SetNull);

    }

    #endregion
}