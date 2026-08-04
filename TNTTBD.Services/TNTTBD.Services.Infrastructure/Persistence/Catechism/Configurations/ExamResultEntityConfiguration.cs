using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Configurations;

/// <summary>
/// Catechist entity configuration
/// </summary>
public class ExamEucharisticYouthEntityConfiguration : IEntityTypeConfiguration<ExamResultEntity>
{
    #region Methods

    /// <summary>
    /// Configure method
    /// </summary>
    public void Configure(EntityTypeBuilder<ExamResultEntity> builder)
    {
        builder.ToTable(nameof(ExamResultEntity));
        builder.HasKey(c => c.Id);

        builder.HasIndex(x => new
        {
            x.ExamId,
            x.CatechismClassEucharisticYouthId,
        }).IsUnique();

        builder.HasOne(er => er.MarkedByCatechist)
            .WithMany()
            .HasForeignKey(x => x.MarkedByCatechistId)
            .OnDelete(DeleteBehavior.SetNull);
    }

    #endregion
}