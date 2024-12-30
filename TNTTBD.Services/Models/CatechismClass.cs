namespace TNTTBD.Services.Models
{
    /// <summary>
    /// Student entity
    /// </summary>
    public class CatechismClass
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public CatechismClass()
        { }

        #endregion

        #region Properties

        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Class Name
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// Primary catechist
        /// </summary>
        public Catechist PrimaryCatechist { get; set; }

        /// <summary>
        /// Catechism Year
        /// </summary>
        public CatechismYear CatechismYear { get; set; }

        /// <summary>
        /// Assistant Catechist List
        /// </summary>
        public List<Catechist> AssistantCatechists { get; set; }  = new List<Catechist>();

        /// <summary>
        /// Catechism Session List
        /// </summary>
        public List<CatechismSession> Sessions { get; set; }  = new List<CatechismSession>();

        #endregion

    }
}