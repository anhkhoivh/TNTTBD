namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

/// <summary>
/// External logins
/// </summary>
public class UserExternalLoginEntity
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Provider { get; set; } = null!; // Google, Microsoft
    public string SubjectId { get; set; } = null!;

    public DateTimeOffset LinkedAt { get; set; }
}