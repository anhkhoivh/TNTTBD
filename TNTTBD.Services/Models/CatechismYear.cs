namespace TNTTBD.Services.Models
{
    /// <summary>
    /// Student entity
    /// </summary>
    public class CatechismYear
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public CatechismYear()
        { }

        #endregion

        #region Properties

        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Start Date
        /// </summary>
        public DateOnly StartDate { get; set; }

        /// <summary>
        /// End Date
        /// </summary>
        public DateOnly EndDate { get; set; }

        /// <summary>
        /// Theme
        /// </summary>
        public string Theme { get; set; }

        /// <summary>
        /// Class in the Catechism Year
        /// </summary>
        public List<CatechismClass> Classes { get; set; } = new List<CatechismClass>();

        #endregion

    }
}