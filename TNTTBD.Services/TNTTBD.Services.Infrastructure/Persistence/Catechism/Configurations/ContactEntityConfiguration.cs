using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Configurations;

/// <summary>
/// Catechist entity configuration
/// </summary>
public class ContactEntityConfiguration : IEntityTypeConfiguration<ContactEntity>
{
    #region Methods

    /// <summary>
    /// Configure method
    /// </summary>
    public void Configure(EntityTypeBuilder<ContactEntity> builder)
    {
        builder.ToTable(nameof(ContactEntity));
        builder.HasKey(c => c.Id);
    }

    #endregion
}