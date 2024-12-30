using System.ComponentModel.DataAnnotations;

namespace TNTTBD.Services.Models
{
    /// <summary>
    /// Student entity
    /// </summary>
    public class Contact
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Contact()
        { }

        #endregion

        #region Properties

        /// <summary>
        /// Id
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Relation
        /// </summary>
        public string Relation { get; set; }

        /// <summary>
        /// Relation
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Phone number
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Eucharistic Youngster that owns the contact
        /// </summary>
        public EucharisticYoungster EucharisticYoungster { get; set; }

        #endregion

    }
}