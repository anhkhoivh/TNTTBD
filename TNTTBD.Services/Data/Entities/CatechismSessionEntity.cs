using System.ComponentModel.DataAnnotations;

namespace TNTTBD.Services.Data.Entities
{
    /// <summary>
    /// Session entity
    /// </summary>
    public class CatechismSessionEntity
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public CatechismSessionEntity()
        { }

        #endregion

        #region Properties

        /// <summary>
        /// Id
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Date
        /// </summary>
        public DateOnly Date { get; set; }

        /// <summary>
        /// Theme
        /// </summary>
        public string Theme { get; set; }

        /// <summary>
        /// Class Id
        /// </summary>
        public Guid ClassId { get; set; }

        /// <summary>
        /// Catechism Class of the Session
        /// </summary>
        public CatechismClassEntity Class { get; set; }

        #endregion

    }
}