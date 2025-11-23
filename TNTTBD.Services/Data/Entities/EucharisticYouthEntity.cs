namespace TNTTBD.Services.Data.Entities
{
    /// <summary>
    /// Student entity
    /// </summary>
    public class EucharisticYouthEntity : MemberEntity
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public EucharisticYouthEntity()
        { }

        #endregion

        #region Properties

        /// <summary>
        /// Date Of Birth
        /// </summary>
        public DateOnly DateOfBirth { get; set; }

        /// <summary>
        /// Date of Baptism
        /// </summary>
        public DateOnly DateOfBaptism { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Zone
        /// </summary>
        public string Zone { get; set; }

        /// <summary>
        /// List of Contacts
        /// </summary>
        public ICollection<ContactEntity> Contacts { get; set; } = new List<ContactEntity>();

        #endregion
    }
}