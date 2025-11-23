using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TNTTBD.Services.Data.Entities
{
    /// <summary>
    /// Student entity
    /// </summary>
    public class ExamResultEntity
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public ExamResultEntity()
        { }

        #endregion

        #region Properties

        /// <summary>
        /// Id
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Point
        /// </summary>
        public double Point { get; set; }

        /// <summary>
        /// Test id
        /// </summary>
        [ForeignKey(nameof(ExamId))]
        public Guid ExamId { get; set; }

        /// <summary>
        /// Exam
        /// </summary>
        public ExamEntity Exam { get; set; }

        /// <summary>
        /// EY id
        /// </summary>
        [ForeignKey(nameof(EucharisticYouthId))]
        public Guid EucharisticYouthId { get; set; }

        /// <summary>
        /// Test
        /// </summary>
        public EucharisticYouthEntity EucharisticYouth { get; set; }

        #endregion
    }
}