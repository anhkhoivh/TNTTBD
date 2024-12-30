using Microsoft.AspNetCore.Identity;
using TNTTBD.Services.Models.Enums;

namespace TNTTBD.Services.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class User : IdentityUser
    {
        #region Constructor

        public User() { }

        #endregion

        #region Properties

        /// <summary>
        /// User Role
        /// </summary>
        public UserRole UserRole { get; set; }

        /// <summary>
        /// Member
        /// </summary>
        public Member Member { get; set; }

        #endregion
    }
}