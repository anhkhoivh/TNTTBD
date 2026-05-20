using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism;

/// <summary>
/// Database context
/// </summary>
public class CatechismDbContext : DbContext
{
    #region Constructors

    public CatechismDbContext(DbContextOptions<CatechismDbContext> options) : base(options)
    { }

    #endregion

    #region Properties

    /// <summary>
    /// Entity of Youngsters
    /// </summary>
    public DbSet<EucharisticYouthEntity> EucharisticYouths => Set<EucharisticYouthEntity>();

    /// <summary>
    /// Entity of Catechists
    /// </summary>
    public DbSet<CatechistEntity> Catechists => Set<CatechistEntity>();

    /// <summary>
    /// Entity of Catechism Years
    /// </summary>
    public DbSet<CatechismYearEntity> CatechismYears => Set<CatechismYearEntity>();

    /// <summary>
    /// Entity of Catechism Classes
    /// </summary>
    public DbSet<CatechismClassEntity> CatechismClasses => Set<CatechismClassEntity>();

    /// <summary>
    /// Entity of Catechism Sessions
    /// </summary>
    public DbSet<CatechismSessionEntity> CatechismSessions => Set<CatechismSessionEntity>();

    /// <summary>
    /// Entity of Milestones
    /// </summary>
    public DbSet<SpiritualMilestoneEntity> SpiritualMilestones => Set<SpiritualMilestoneEntity>();

    /// <summary>
    /// Entity of Contacts
    /// </summary>
    public DbSet<ContactEntity> Contacts => Set<ContactEntity>();

    /// <summary>
    /// Entity of Contact and EucharisticYouth
    /// </summary>
    public DbSet<EucharisticYouthContactEntity> EucharisticYouthContacts => Set<EucharisticYouthContactEntity>();

    /// <summary>
    /// Entity of Exams
    /// </summary>
    public DbSet<ExamEntity> Exams => Set<ExamEntity>();

    /// <summary>
    /// Entity of ExamResults
    /// </summary>
    public DbSet<ExamResultEntity> ExamResults => Set<ExamResultEntity>();

    /// <summary>
    /// Entity of CatechismSessionAttendance
    /// </summary>
    public DbSet<CatechismSessionAttendanceEntity> CatechismSessionAttendances => Set<CatechismSessionAttendanceEntity>();

    /// <summary>
    /// Join entity CatechismClassCatechistEntity
    /// </summary>
    public DbSet<CatechismClassCatechistEntity> CatechismClassCatechists => Set<CatechismClassCatechistEntity>();

    /// <summary>
    /// Join entity CatechismClassEucharisticYouthEntity
    /// </summary>
    public DbSet<CatechismClassEucharisticYouthEntity> CatechismClassEucharisticYouths => Set<CatechismClassEucharisticYouthEntity>();

    /// <summary>
    /// Entity of Users
    /// </summary>
    public DbSet<UserEntity> Users => Set<UserEntity>();

    /// <summary>
    /// Entity of User external login
    /// </summary>
    public DbSet<UserExternalLoginEntity> UserExternalLogins => Set<UserExternalLoginEntity>();

    /// <summary>
    /// Entity of EucharisticYouthRunningNumber
    /// </summary>
    public DbSet<EucharisticYouthRunningNumberEntity> EucharisticYouthRunningNumbers => Set<EucharisticYouthRunningNumberEntity>();

    /// <summary>
    /// Entity of CatechistPhotoEntity
    /// </summary>
    public DbSet<CatechistPhotoEntity> CatechistPhotos => Set<CatechistPhotoEntity>();

    /// <summary>
    /// Entity of CatechistPhotoEntity
    /// </summary>
    public DbSet<EucharisticYouthPhotoEntity> EucharisticYouthPhotos => Set<EucharisticYouthPhotoEntity>();

    #endregion

    #region Methods

    /// <summary>
    /// On model creating
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(CatechismDbContext).Assembly
        );
    }

    #endregion
}
