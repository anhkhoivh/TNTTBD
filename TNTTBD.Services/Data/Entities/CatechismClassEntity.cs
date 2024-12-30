using System.ComponentModel.DataAnnotations;

namespace TNTTBD.Services.Data.Entities
{
    /// <summary>
    /// Student entity
    /// </summary>
    public class CatechismClassEntity
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public CatechismClassEntity()
        { }

        #endregion

        #region Properties

        /// <summary>
        /// Id
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Class Name
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// Primary catechist id
        /// </summary>
        public Guid PrimaryCatechistId { get; set; }

        /// <summary>
        /// Primary catechist
        /// </summary>
        public CatechistEntity PrimaryCatechist { get; set; }

        /// <summary>
        /// Catechism Year Id
        /// </summary>
        public Guid CatechismYearId { get; set; }

        /// <summary>
        /// Catechism Year
        /// </summary>
        public CatechismYearEntity CatechismYear { get; set; }

        /// <summary>
        /// Assistant Catechist List
        /// </summary>
        public ICollection<CatechistEntity> AssistantCatechists { get; set; }  = new List<CatechistEntity>();

        /// <summary>
        /// Catechism Session List
        /// </summary>
        public ICollection<CatechismSessionEntity> Sessions { get; set; }  = new List<CatechismSessionEntity>();

        #endregion

    }
}