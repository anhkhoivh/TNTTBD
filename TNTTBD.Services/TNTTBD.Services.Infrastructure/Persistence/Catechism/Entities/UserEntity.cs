using Microsoft.AspNetCore.Identity;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

/// <summary>
/// 
/// </summary>
public class UserEntity : IdentityUser
{
    #region Constructor

    public UserEntity() { }

    #endregion

    #region Properties

    /// <summary>
    /// User Role
    /// </summary>
    public UserRole UserRole { get; set; }

    #endregion
}
