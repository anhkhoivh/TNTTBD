using TNTTBD.Services.Domain.Common;

namespace TNTTBD.Services.Domain.Catechism;

public sealed class UserExternalLogin : DomainEntity<Guid>
{
    #region Constructions

    public UserExternalLogin(
        Guid id,
        Guid userId,
        string provider,
        string subjectId, 
        DateTimeOffset linkedAt)
        : base(id)
    {
        UserId = userId;
        Provider = provider;
        SubjectId = subjectId;
        LinkedAt = linkedAt;
    }

    #endregion

    #region Properties

    /// <summary>
    /// User id
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    /// provider
    /// </summary>
    public string Provider { get; }

    /// <summary>
    /// Subject id
    /// </summary>
    public string SubjectId { get; }

    /// <summary>
    /// Linked at
    /// </summary>
    public DateTimeOffset LinkedAt { get; }

    #endregion

    #region Methods

    public static UserExternalLogin Create(
        Guid userId,
        string provider,
        string subjectId)
    {
        return new UserExternalLogin(
            Guid.NewGuid(),
            userId,
            provider,
            subjectId,
            DateTimeOffset.Now.ToUniversalTime());
    }

    #endregion
}
