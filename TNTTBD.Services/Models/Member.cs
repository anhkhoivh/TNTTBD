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
        /// Family Name
        /// </summary>
        public string FamilyName { get; set; }

        /// <summary>
        /// Middle Name
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Given Name
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        /// User
        /// </summary>
        public User User { get; set; }
        
        /// <summary>
        /// Full Name
        /// </summary>
        public string FullName => $"{ChristName} {FamilyName} {MiddleName} {GivenName}";

        #endregion
    }
}