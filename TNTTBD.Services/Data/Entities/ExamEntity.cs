using System.ComponentModel.DataAnnotations;

namespace TNTTBD.Services.Data.Entities
{
    /// <summary>
    /// Student entity
    /// </summary>
    public class ExamEntity
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public ExamEntity()
        { }

        #endregion

        #region Properties

        /// <summary>
        /// Id
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Class Id
        /// </summary>
        public Guid ClassId { get; set; }

        /// <summary>
        /// Class
        /// </summary>
        public CatechismClassEntity Class { get; set; }

        /// <summary>
        /// Exam Result list
        /// </summary>
        public ICollection<ExamResultEntity> ExamResults { get; set; } = new List<ExamResultEntity>();

        #endregion
    }
}