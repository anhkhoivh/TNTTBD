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
        public DbSet<UserEntity> Users { get; set; }

        /// <summary>
        /// Entity of Youngsters
        /// </summary>
        public DbSet<EucharisticYoungsterEntity> EucharisticYoungsters { get; set; }

        /// <summary>
        /// Entity of Catechists
        /// </summary>
        public DbSet<CatechistEntity> Catechists { get; set; }

        /// <summary>
        /// Entity of Catechism Years
        /// </summary>
        public DbSet<CatechismYearEntity> CatechismYears { get; set; }

        /// <summary>
        /// Entity of Catechism Classes
        /// </summary>
        public DbSet<CatechismClassEntity> CatechismClasses { get; set; }

        /// <summary>
        /// Entity of Catechism Sessions
        /// </summary>
        public DbSet<CatechismSessionEntity> CatechismSessions { get; set; }

        /// <summary>
        /// Entity of Contacts
        /// </summary>
        public DbSet<ContactEntity> Contacts { get; set; }

        /// <summary>
        /// Entity of Exams
        /// </summary>
        public DbSet<ExamEntity> Exams { get; set; }

        /// <summary>
        /// Entity of ExamResults
        /// </summary>
        public DbSet<ExamResultEntity> ExamResults { get; set; }

        #endregion
    }
}