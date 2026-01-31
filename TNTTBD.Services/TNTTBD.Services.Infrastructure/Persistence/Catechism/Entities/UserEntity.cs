using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

public sealed class UserEntity
{
    #region Properties

    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;
    public string Email { get; set; } = null!;

    public string? PasswordHash { get; set; }

    public bool IsActive { get; set; }

    public Guid? CatechistId { get; set; }
    public Guid? ContactId { get; set; }
    public Guid? EucharisticYouthId { get; set; }

    // Authorization
    public UserRole Role { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public ICollection<UserExternalLoginEntity> ExternalLogins { get; set; } = new List<UserExternalLoginEntity>();

    #endregion
}