using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Configurations;

/// <summary>
/// Catechist entity configuration
/// </summary>
public class ExamEntityConfiguration : IEntityTypeConfiguration<ExamEntity>
{
    #region Methods

    /// <summary>
    /// Configure method
    /// </summary>
    public void Configure(EntityTypeBuilder<ExamEntity> builder)
    {
        builder.ToTable(nameof(ExamEntity));
        builder.HasKey(c => c.Id);
        
        builder.HasOne(e => e.CatechismClass)
            .WithMany(c => c.Exams)
            .HasForeignKey(c => c.CatechismClassId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.ExamResults)
            .WithOne(r => r.Exam)
            .HasForeignKey(e => e.ExamId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    #endregion
}