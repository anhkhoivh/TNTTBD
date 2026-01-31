using System.ComponentModel.DataAnnotations;
using TNTTBD.Services.Domain.Common;

namespace TNTTBD.Services.Domain.Catechism;

/// <summary>
/// Student entity
/// </summary>
public class Exam : AggregateRoot<Guid>
{
    #region Fields

    private readonly List<ExamResult> _examResults = new List<ExamResult>();

    #endregion

    #region Constructors

    /// <summary>
    /// Default constructor
    /// </summary>
    public Exam() : base(Guid.Empty)
    {
        Title = string.Empty;
    }

    /// <summary>
    /// Constructor from details
    /// </summary>
    public Exam(Guid id, Guid catechismClassId, DateOnly examDate, string title) : base(id)
    {
        CatechismClassId = catechismClassId;
        ExamDate = examDate;
        Title = title;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Title
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// CatechismClassId
    /// </summary>
    public Guid CatechismClassId { get; private set; }

    /// <summary>
    /// Exam Date
    /// </summary>
    public DateOnly ExamDate { get; private set; }

    /// <summary>
    /// Results
    /// </summary>
    public IReadOnlyCollection<ExamResult> ExamResults => _examResults;

    #endregion

    #region Methods

    /// <summary>
    /// Mark attendance
    /// </summary>
    public void MarkResult(
        Guid catechismClassEucharisticYouthId,
        double score,
        Guid markedBy, 
        bool isDone,
        string? remark)
    {
        if (_examResults.Any(x => x.CatechismClassEucharisticYouthId == catechismClassEucharisticYouthId))
        {
            throw new DomainException("Attendance already recorded.");
        }

        _examResults.Add(new ExamResult(
            Guid.NewGuid(),
            Id,
            catechismClassEucharisticYouthId,
            markedBy,
            DateTimeOffset.Now,
            score,
            isDone,
            remark
        ));
    }

    #endregion
}
