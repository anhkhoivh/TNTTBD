using TNTTBD.Services.Domain.Common;

namespace TNTTBD.Services.Domain.Catechism;

public sealed class User : AggregateRoot<Guid>
{
    #region Fields

    private readonly List<UserExternalLogin> _externalLogins = new List<UserExternalLogin>();

    #endregion

    #region Constructions

    public User(Guid id, string email, string userName)
        : base(id)
    {
        Email = email;
        UserName = userName;
        IsActive = true;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; private set; }

    /// <summary>
    /// Username
    /// </summary>
    public string UserName { get; private set; }

    /// <summary>
    /// Is active
    /// </summary>
    public bool IsActive { get; private set; }

    public Guid? CatechistId { get; private set; }
    public Guid? EucharisticYouthId { get; private set; }
    public Guid? ContactId { get; private set; }

    /// <summary>
    /// External login
    /// </summary>
    public IReadOnlyCollection<UserExternalLogin> ExternalLogins => _externalLogins;

    #endregion

    #region Methods

    /// <summary>
    /// Create
    /// </summary>
    public static User Create(string email, string userName)
    {
        return new User(Guid.NewGuid(), email, userName);
    }

    /// <summary>
    /// Deactivate
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
    }

    /// <summary>
    /// Activate
    /// </summary>
    public void Activate()
    {
        IsActive = true;
    }

    /// <summary>
    /// Link catechist
    /// </summary>
    /// <param name="catechistId"></param>
    public void LinkCatechist(Guid catechistId)
    {
        CatechistId = catechistId;
    }

    /// <summary>
    /// Link contact
    /// </summary>
    public void LinkContact(Guid contactId)
    {
        ContactId = contactId;
    }

    /// <summary>
    /// Link 
    /// </summary>
    public void LinkEucharisticYouth(Guid youthId)
    {
        EucharisticYouthId = youthId;
    }

    /// <summary>
    /// Add external login
    /// </summary>
    public void AddExternalLogin(
        string provider,
        string subjectId)
    {
        if (_externalLogins.Any(x =>
            x.Provider == provider && x.SubjectId == subjectId))
        {
            return;
        }

        _externalLogins.Add(
            UserExternalLogin.Create(
                Id,
                provider,
                subjectId));
    }

    #endregion
}
