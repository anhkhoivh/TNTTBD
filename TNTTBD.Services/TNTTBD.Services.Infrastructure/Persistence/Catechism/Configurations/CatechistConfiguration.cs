using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TNTTBD.Services.Domain.Members;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Configurations;

/// <summary>
/// Catechist entity configuration
/// </summary>
public class CatechistConfiguration : IEntityTypeConfiguration<CatechistEntity>
{
    #region Methods

    /// <summary>
    /// Configure method
    /// </summary>
    public void Configure(EntityTypeBuilder<CatechistEntity> builder)
    {
        builder.ToTable(nameof(Catechist));
        builder.HasKey(c => c.Id);

        builder.HasMany(c => c.CatechismClassCatechists)
            .WithOne(cc => cc.Catechist)
            .HasForeignKey(cc => cc.CatechistId);
    }

    #endregion
}