using Microsoft.AspNetCore.Identity;
using TNTTBD.Services.Models.Enums;

namespace TNTTBD.Services.Data.Entities
{
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

        /// <summary>
        /// Member id
        /// </summary>
        public Guid MemberId { get; set; }

        /// <summary>
        /// Member
        /// </summary>
        public MemberEntity Member { get; set; }

        #endregion
    }
}