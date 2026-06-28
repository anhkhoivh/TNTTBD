using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Configurations;

/// <summary>
/// Catechist entity configuration
/// </summary>
public class CatechismSessionEntityConfiguration : IEntityTypeConfiguration<CatechismSessionEntity>
{
    #region Methods

    /// <summary>
    /// Configure method
    /// </summary>
    public void Configure(EntityTypeBuilder<CatechismSessionEntity> builder)
    {
        builder.ToTable(nameof(CatechismSessionEntity));
        builder.HasKey(c => c.Id);

        builder.HasMany(c => c.EucharisticYouthAttendances)
            .WithOne(a => a.CatechismSession)
            .HasForeignKey(attendance => attendance.CatechismSessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(s => s.CatechismClass)
            .WithMany(c => c.CatechismSessions)
            .HasForeignKey(l => l.CatechismClassId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(l => l.EucharisticYouthAttendances)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }

    #endregion
}