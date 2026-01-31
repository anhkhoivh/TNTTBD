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

        builder.HasOne<CatechismYearEntity>()
            .WithMany(year => year.CatechismClasses)
            .HasForeignKey(year => year.CatechismYearId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany<CatechismClassEucharisticYouthEntity>()
            .WithOne()
            .HasForeignKey(ce => ce.CatechismClassId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany<CatechismClassCatechistEntity>()
            .WithOne()
            .HasForeignKey(cc => cc.CatechismClassId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.CatechismSessions)
            .WithOne()
            .HasForeignKey(session => session.CatechismClassId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Exams)
            .WithOne()
            .HasForeignKey(exam => exam.CatechismClassId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    #endregion
}