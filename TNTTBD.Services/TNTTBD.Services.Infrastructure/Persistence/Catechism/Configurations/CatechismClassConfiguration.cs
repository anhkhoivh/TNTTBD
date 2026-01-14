using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TNTTBD.Services.Domain.Catechism;
using TNTTBD.Services.Domain.Members;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Configurations;

/// <summary>
/// Catechist entity configuration
/// </summary>
public class CatechismClassConfiguration : IEntityTypeConfiguration<CatechismClassEntity>
{
    #region Methods

    /// <summary>
    /// Configure method
    /// </summary>
    public void Configure(EntityTypeBuilder<CatechismClassEntity> builder)
    {
        builder.ToTable(nameof(CatechismClass));
        builder.HasKey(c => c.Id);

        builder.HasMany(c => c.CatechismClassEucharisticYouths)
            .WithOne(ce => ce.CatechismClass)
            .HasForeignKey(ce => ce.CatechismClassId);

        builder.HasMany(c => c.CatechismClassCatechists)
            .WithOne(cc => cc.CatechismClass)
            .HasForeignKey(cc => cc.CatechismClassId);
    }

    #endregion
}