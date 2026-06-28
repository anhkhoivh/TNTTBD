using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Configurations;

/// <summary>
/// Catechist entity configuration
/// </summary>
public class CatechismSessionAttendanceEntityConfiguration : IEntityTypeConfiguration<CatechismSessionAttendanceEntity>
{
    #region Methods

    /// <summary>
    /// Configure method
    /// </summary>
    public void Configure(EntityTypeBuilder<CatechismSessionAttendanceEntity> builder)
    {
        builder.ToTable(nameof(CatechismSessionAttendanceEntity));
        builder.HasKey(a => a.Id);

        builder.HasIndex(x => new
        {
            x.CatechismSessionId,
            x.CatechismClassEucharisticYouthId
        }).IsUnique();

        builder.HasOne(sa => sa.CatechismSession)
            .WithMany(s => s.EucharisticYouthAttendances)
            .HasForeignKey(s => s.CatechismSessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(sa => sa.CatechismClassEucharisticYouth)
            .WithMany(cy => cy.EucharisticYouthAttendances)
            .HasForeignKey(x => x.CatechismClassEucharisticYouthId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(sa => sa.MarkedByCatechist)
            .WithMany()
            .HasForeignKey(x => x.MarkedByCatechistId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    #endregion
}