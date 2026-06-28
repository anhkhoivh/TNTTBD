using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Configurations;

/// <summary>
/// Catechist entity configuration
/// </summary>
public class CatechistPhotoEntityConfiguration : IEntityTypeConfiguration<CatechistPhotoEntity>
{
    #region Methods

    /// <summary>
    /// Configure method
    /// </summary>
    public void Configure(EntityTypeBuilder<CatechistPhotoEntity> builder)
    {
        builder.ToTable(nameof(CatechistPhotoEntity));
        builder.HasKey(c => c.Id);

        builder.HasOne(p => p.Catechist)
            .WithMany(c => c.Photos)
            .HasForeignKey(c => c.CatechistId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    #endregion
}