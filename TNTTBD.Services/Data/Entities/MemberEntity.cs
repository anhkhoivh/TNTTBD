using System.ComponentModel.DataAnnotations;

namespace TNTTBD.Services.Data.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class MemberEntity
    {
        #region Constructor

        public MemberEntity() {}

        #endregion

        #region Properties

        /// <summary>
        /// Id
        /// </summary>
        [Key]
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
        /// User id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// User
        /// </summary>
        public UserEntity User { get; set; }

        #endregion
    }
}