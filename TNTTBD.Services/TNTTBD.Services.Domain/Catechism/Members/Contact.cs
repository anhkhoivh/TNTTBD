using TNTTBD.Services.Domain.Common;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Domain.Catechism.Members;

/// <summary>
/// Student entity
/// </summary>
public class Contact : AggregateRoot<Guid>
{
    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public Contact() : base(Guid.Empty)
    {
        FullName = new FullName();
        PhoneNumber = string.Empty;
        Email = string.Empty;
    }

    /// <summary>
    /// Constructor with details
    /// </summary>
    public Contact(Guid id, FullName fullName, string? phoneNumber, string? email) : base(id)
    {
        FullName = fullName;
        PhoneNumber = phoneNumber ?? string.Empty;
        Email = email ?? string.Empty;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Name
    /// </summary>
    public FullName FullName { get; protected set; }

    /// <summary>
    /// Phone number
    /// </summary>
    public string PhoneNumber { get; protected set; }

    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; protected set; }

    #endregion
}
