using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Configurations;

/// <summary>
/// Catechist entity configuration
/// </summary>
public class CatechismClassEntityConfiguration : IEntityTypeConfiguration<CatechismClassEntity>
{
    #region Methods

    /// <summary>
    /// Configure method
    /// </summary>
    public void Configure(EntityTypeBuilder<CatechismClassEntity> builder)
    {
        builder.ToTable(nameof(CatechismClassEntity));
        builder.HasKey(c => c.Id);

        builder.HasMany(c => c.CatechismClassEucharisticYouths)
            .WithOne(cy => cy.CatechismClass)
            .HasForeignKey(ce => ce.CatechismClassId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.CatechismClassCatechists)
            .WithOne(cc => cc.CatechismClass)
            .HasForeignKey(cc => cc.CatechismClassId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.CatechismSessions)
            .WithOne(s => s.CatechismClass)
            .HasForeignKey(session => session.CatechismClassId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Exams)
            .WithOne(e => e.CatechismClass)
            .HasForeignKey(exam => exam.CatechismClassId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    #endregion
}