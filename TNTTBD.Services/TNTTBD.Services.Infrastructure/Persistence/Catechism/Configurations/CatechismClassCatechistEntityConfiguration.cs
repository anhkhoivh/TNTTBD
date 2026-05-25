using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Configurations;

/// <summary>
/// Catechist entity configuration
/// </summary>
public class CatechismClassCatechistEntityConfiguration : IEntityTypeConfiguration<CatechismClassCatechistEntity>
{
    #region Methods

    /// <summary>
    /// Configure method
    /// </summary>
    public void Configure(EntityTypeBuilder<CatechismClassCatechistEntity> builder)
    {
        builder.ToTable(nameof(CatechismClassCatechistEntity));
        builder.HasKey(c => c.Id);

        builder.HasIndex(x => new
        {
            x.CatechismClassId,
            x.CatechistId,
        }).IsUnique();

        builder.HasOne<CatechismClassEntity>()
            .WithMany()
            .HasForeignKey(c => c.CatechismClassId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<CatechistEntity>()
            .WithMany()
            .HasForeignKey(c => c.CatechistId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    #endregion
}