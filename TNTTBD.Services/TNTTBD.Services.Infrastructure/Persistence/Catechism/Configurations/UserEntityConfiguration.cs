using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Configurations;

/// <summary>
/// Catechist entity configuration
/// </summary>
public class UserEntityConfiguration : IEntityTypeConfiguration<UserEntity>
{
    #region Methods

    /// <summary>
    /// Configure method
    /// </summary>
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable(nameof(UserEntity));
        builder.HasKey(c => c.Id);

        builder.HasIndex(u => u.UserName)
            .IsUnique();
        
        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.HasMany(u => u.ExternalLogins)
            .WithOne(l => l.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(u => u.Catechist)
            .WithOne(c => c.User)
            .HasForeignKey<UserEntity>(u => u.CatechistId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(u => u.Contact)
            .WithOne(c => c.User)
            .HasForeignKey<UserEntity>(u => u.ContactId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(u => u.EucharisticYouth)
            .WithOne(y => y.User)
            .HasForeignKey<UserEntity>(u => u.EucharisticYouthId)
            .OnDelete(DeleteBehavior.Cascade);

    }

    #endregion
}