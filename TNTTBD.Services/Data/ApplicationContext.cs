using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Data.Entities;

namespace TNTTBD.Services.Data
{
    /// <summary>
    /// Database context
    /// </summary>
    public class ApplicationContext : DbContext
    {
        #region Constructors

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        { }

        #endregion

        #region Properties

        /// <summary>
        /// Entity of Users
        /// </summary>
        public DbSet<UserEntity> User { get; set; }

        /// <summary>
        /// Entity of Youngsters
        /// </summary>
        public DbSet<EucharisticYouthEntity> EucharisticYouth { get; set; }

        /// <summary>
        /// Entity of Catechists
        /// </summary>
        public DbSet<CatechistEntity> Catechist { get; set; }

        /// <summary>
        /// Entity of Catechism Years
        /// </summary>
        public DbSet<CatechismYearEntity> CatechismYear { get; set; }

        /// <summary>
        /// Entity of Catechism Classes
        /// </summary>
        public DbSet<CatechismClassEntity> CatechismClass { get; set; }

        /// <summary>
        /// Entity of Catechism Sessions
        /// </summary>
        public DbSet<CatechismSessionEntity> CatechismSession { get; set; }

        /// <summary>
        /// Entity of Contacts
        /// </summary>
        public DbSet<ContactEntity> Contact { get; set; }

        /// <summary>
        /// Entity of Exams
        /// </summary>
        public DbSet<ExamEntity> Exam { get; set; }

        /// <summary>
        /// Entity of ExamResults
        /// </summary>
        public DbSet<ExamResultEntity> ExamResult { get; set; }

        #endregion
    }
}