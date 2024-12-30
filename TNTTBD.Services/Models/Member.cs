using System.ComponentModel.DataAnnotations;

namespace TNTTBD.Services.Models
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Member
    {
        #region Constructor

        public Member() {}

        #endregion

        #region Properties

        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Christ Name
        /// </summary>
        public string ChristName { get; set; }

        /// <summary>
        /// Full Name
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// User
        /// </summary>
        public User User {get;set;}

        #endregion
    }
}