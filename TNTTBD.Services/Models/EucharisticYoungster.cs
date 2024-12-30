namespace TNTTBD.Services.Models
{
    /// <summary>
    /// Student entity
    /// </summary>
    public class EucharisticYoungster : Member
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public EucharisticYoungster()
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
        public List<Contact> Contacts { get; set; } = new List<Contact>();

        #endregion

    }
}