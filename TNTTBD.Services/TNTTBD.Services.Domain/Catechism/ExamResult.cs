using TNTTBD.Services.Domain.Common;

namespace TNTTBD.Services.Domain.Catechism;

/// <summary>
/// Student entity
/// </summary>
public class ExamResult : DomainEntity<Guid>
{
    #region Constructors

    public ExamResult(
        Guid id,
        Guid examId,
        Guid catechismClassEucharisticYouthId,
        Guid markedByCatechistId,
        DateTimeOffset markedTime,
        double score,
        bool isDone,
        string? remark) : base(id)
    {
        CatechismClassEucharisticYouthId = catechismClassEucharisticYouthId;
        ExamId = examId;
        MarkedByCatechistId = markedByCatechistId;
        Score = score;
        IsDone = isDone;
        Remark = remark;
        MarkedTime = markedTime;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Score
    /// </summary>
    public double Score { get; private set; }

    /// <summary>
    /// Exam id
    /// </summary>
    public Guid ExamId { get; private set; }

    /// <summary>
    /// CatechismClassEucharisticYouth
    /// </summary>
    public Guid CatechismClassEucharisticYouthId { get; }

    /// <summary>
    /// Marked by
    /// </summary>
    public Guid MarkedByCatechistId { get; private set; }

    /// <summary>
    /// Is done
    /// </summary>
    public bool IsDone { get; private set; }

    /// <summary>
    /// Is done
    /// </summary>
    public string? Remark { get; private set; }

    /// <summary>
    /// Marked at
    /// </summary>
    public DateTimeOffset MarkedTime { get; private set; }

    #endregion

    #region Methods

    /// <summary>
    /// Update
    /// </summary>
    public void Update(
        double score,
        bool isDone,
        string? remark,
        DateTimeOffset markedTime,
        Guid markedByCatechistId)
    {
        Score = score;
        MarkedTime = markedTime;
        MarkedByCatechistId = markedByCatechistId;
        IsDone = isDone;
        Remark = remark;
    }

    #endregion
}
