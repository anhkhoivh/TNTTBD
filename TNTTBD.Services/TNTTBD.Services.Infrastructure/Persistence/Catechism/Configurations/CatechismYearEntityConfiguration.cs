using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Configurations;

/// <summary>
/// Catechist entity configuration
/// </summary>
public class CatechismYearEntityConfiguration : IEntityTypeConfiguration<CatechismYearEntity>
{
    #region Methods

    /// <summary>
    /// Configure method
    /// </summary>
    public void Configure(EntityTypeBuilder<CatechismYearEntity> builder)
    {
        builder.ToTable(nameof(CatechismYearEntity));
        builder.HasKey(c => c.Id);

        builder.HasMany(year => year.CatechismClasses)
            .WithOne(c => c.CatechismYear)
            .HasForeignKey(c => c.CatechismYearId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    #endregion
}