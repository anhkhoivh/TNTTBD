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
        /// Full Name
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// User id
        /// </summary>
        public Guid UserId {get;set;}

        /// <summary>
        /// User
        /// </summary>
        public UserEntity User {get;set;}

        #endregion
    }
}