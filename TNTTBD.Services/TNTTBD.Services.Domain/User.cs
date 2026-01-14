using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class User
    {
        #region Constructor

        public User() { }

        #endregion

        #region Properties

        /// <summary>
        /// User Role
        /// </summary>
        public UserRole UserRole { get; set; }

        #endregion
    }
}