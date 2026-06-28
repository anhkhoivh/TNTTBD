using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Configurations;

public class UserExternalLoginEntityConfiguration
    : IEntityTypeConfiguration<UserExternalLoginEntity>
{
    public void Configure(EntityTypeBuilder<UserExternalLoginEntity> builder)
    {
        builder.ToTable(nameof(UserExternalLoginEntity));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.Provider)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.SubjectId)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(x => x.LinkedAt)
            .IsRequired();

        // Critical index for SSO lookup
        builder.HasIndex(x => new { x.Provider, x.SubjectId })
            .IsUnique();

        // Optional but useful
        builder.HasIndex(x => x.UserId);

        builder.HasOne(l => l.User)
            .WithMany(u => u.ExternalLogins)
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
