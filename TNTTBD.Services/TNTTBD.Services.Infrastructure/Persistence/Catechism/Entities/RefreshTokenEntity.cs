using System.ComponentModel.DataAnnotations;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

public class RefreshTokenEntity
{
    [Key]
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string TokenHash { get; set; } = null!;
    public DateTimeOffset ExpiresAt { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public bool IsRevoked { get; set; }
}
