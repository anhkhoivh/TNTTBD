using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Application.Reports.DTOs;
using TNTTBD.Services.Application.Reports.Interfaces;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.QueryServices;

public class ReportQueryService(CatechismDbContext context) : IReportQueryService
{
    public async Task<ClassReportDto?> GetClassReportAsync(Guid classId, CancellationToken cancellationToken)
    {
        var classInfo = await context.CatechismClasses
            .AsNoTracking()
            .Where(c => c.Id == classId)
            .Select(c => new
            {
                c.Id,
                c.ClassCode,
                c.ClassName,
                c.CatechismYearId,
                YearTheme = c.CatechismYear.Theme
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (classInfo == null)
        {
            return null;
        }

        var now = DateTimeOffset.UtcNow;

        var sessions = await context.CatechismSessions
            .AsNoTracking()
            .Where(s => s.CatechismClassId == classId)
            .OrderBy(s => s.DateTime)
            .Select(s => new ClassReportSessionDto
            {
                SessionId = s.Id,
                Title = s.Title,
                DateTime = s.DateTime
            })
            .ToListAsync(cancellationToken);

        foreach (var session in sessions)
        {
            session.IsHeld = session.DateTime <= now;
        }

        var exams = await context.Exams
            .AsNoTracking()
            .Where(e => e.CatechismClassId == classId)
            .OrderBy(e => e.ExamDate)
            .ThenBy(e => e.Title)
            .Select(e => new ClassReportExamDto
            {
                ExamId = e.Id,
                Title = e.Title,
                ExamDate = e.ExamDate
            })
            .ToListAsync(cancellationToken);

        var enrollments = await context.CatechismClassEucharisticYouths
            .AsNoTracking()
            .Where(e => e.CatechismClassId == classId)
            .OrderBy(e => e.EucharisticYouth.FullName.GivenName)
            .ThenBy(e => e.EucharisticYouth.FullName.FamilyName)
            .Select(e => new
            {
                e.Id,
                e.EucharisticYouthId,
                e.EucharisticYouth.Code,
                e.EucharisticYouth.FullName,
                e.Status,
                e.EnrolledAt,
                e.LeftAt
            })
            .ToListAsync(cancellationToken);

        var attendances = await context.CatechismSessionAttendances
            .AsNoTracking()
            .Where(a => a.CatechismSession.CatechismClassId == classId)
            .Select(a => new { a.CatechismSessionId, a.CatechismClassEucharisticYouthId, a.AttendanceStatus })
            .ToListAsync(cancellationToken);

        var statusLookup = new Dictionary<(Guid SessionId, Guid EnrollmentId), AttendanceStatus>();
        foreach (var attendance in attendances)
        {
            statusLookup.TryAdd((attendance.CatechismSessionId, attendance.CatechismClassEucharisticYouthId), attendance.AttendanceStatus);
        }

        var results = await context.ExamResults
            .AsNoTracking()
            .Where(r => r.Exam.CatechismClassId == classId)
            .Select(r => new { r.ExamId, r.CatechismClassEucharisticYouthId, r.Score, r.Remark })
            .ToListAsync(cancellationToken);

        var resultLookup = new Dictionary<(Guid ExamId, Guid EnrollmentId), (double? Score, string? Remark)>();
        foreach (var result in results)
        {
            resultLookup.TryAdd((result.ExamId, result.CatechismClassEucharisticYouthId), (result.Score, result.Remark));
        }

        var rows = enrollments.Select(enrollment =>
        {
            var cells = sessions
                .Select(session => new ClassReportAttendanceCellDto
                {
                    SessionId = session.SessionId,
                    Status = statusLookup.GetValueOrDefault((session.SessionId, enrollment.Id), AttendanceStatus.None),
                    IsCounted = IsCounted(session.DateTime, enrollment.EnrolledAt, enrollment.LeftAt, now)
                })
                .ToList();

            var scores = exams
                .Select(exam =>
                {
                    resultLookup.TryGetValue((exam.ExamId, enrollment.Id), out var result);
                    return new ClassReportScoreCellDto
                    {
                        ExamId = exam.ExamId,
                        Score = result.Score,
                        Remark = result.Remark
                    };
                })
                .ToList();

            var summary = CreateAttendanceSummary(cells.Where(c => c.IsCounted).Select(c => c.Status));
            var scoredValues = scores.Where(s => s.Score.HasValue).Select(s => s.Score!.Value).ToList();

            return new ClassReportRowDto
            {
                EnrollmentId = enrollment.Id,
                YouthId = enrollment.EucharisticYouthId,
                YouthCode = enrollment.Code,
                YouthFullName = enrollment.FullName,
                EnrollmentStatus = enrollment.Status,
                EnrolledAt = enrollment.EnrolledAt,
                LeftAt = enrollment.LeftAt,
                Attendances = cells,
                Scores = scores,
                Counted = summary.SessionsHeld,
                Present = summary.Present,
                Absent = summary.Absent,
                Excused = summary.Excused,
                Unmarked = summary.Unmarked,
                AttendanceRate = summary.AttendanceRate,
                Scored = scoredValues.Count,
                AverageScore = Average(scoredValues)
            };
        }).ToList();

        return new ClassReportDto
        {
            ClassId = classInfo.Id,
            ClassCode = classInfo.ClassCode,
            ClassName = classInfo.ClassName,
            CatechismYearId = classInfo.CatechismYearId,
            CatechismYearName = classInfo.YearTheme,
            SessionsHeld = sessions.Count(s => s.IsHeld),
            Sessions = sessions,
            Exams = exams,
            Rows = rows
        };
    }

    public async Task<YouthReportDto?> GetYouthReportAsync(
        Guid youthId,
        Guid? catechismYearId,
        Guid? enrollmentId,
        CancellationToken cancellationToken)
    {
        var youth = await context.EucharisticYouths
            .AsNoTracking()
            .Where(y => y.Id == youthId)
            .Select(y => new { y.Id, y.Code, y.FullName })
            .FirstOrDefaultAsync(cancellationToken);

        if (youth == null)
        {
            return null;
        }

        if (enrollmentId.HasValue)
        {
            var enrollmentYearId = await context.CatechismClassEucharisticYouths
                .AsNoTracking()
                .Where(e => e.Id == enrollmentId.Value && e.EucharisticYouthId == youthId)
                .Select(e => (Guid?)e.CatechismClass.CatechismYearId)
                .FirstOrDefaultAsync(cancellationToken);

            if (!enrollmentYearId.HasValue)
            {
                return null;
            }

            catechismYearId = enrollmentYearId;
        }
        else if (!catechismYearId.HasValue)
        {
            catechismYearId = await context.CatechismYears
                .AsNoTracking()
                .OrderByDescending(y => y.StartDate)
                .Select(y => (Guid?)y.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        var enrollmentQuery = context.CatechismClassEucharisticYouths
            .AsNoTracking()
            .Where(e => e.EucharisticYouthId == youthId);

        if (catechismYearId.HasValue)
        {
            enrollmentQuery = enrollmentQuery.Where(e => e.CatechismClass.CatechismYearId == catechismYearId.Value);
        }

        var enrollments = await enrollmentQuery
            .OrderByDescending(e => e.EnrolledAt)
            .Select(e => new YouthClassReportDto
            {
                EnrollmentId = e.Id,
                ClassId = e.CatechismClassId,
                ClassCode = e.CatechismClass.ClassCode,
                ClassName = e.CatechismClass.ClassName,
                CatechismYearId = e.CatechismClass.CatechismYearId,
                CatechismYearName = e.CatechismClass.CatechismYear.Theme,
                EnrollmentStatus = e.Status,
                EnrolledAt = e.EnrolledAt,
                LeftAt = e.LeftAt,
                Note = e.Note
            })
            .ToListAsync(cancellationToken);

        var scoped = enrollmentId.HasValue
            ? enrollments.Where(e => e.EnrollmentId == enrollmentId.Value).ToList()
            : enrollments;
        var classIds = scoped.Select(e => e.ClassId).Distinct().ToList();
        var scopedIds = scoped.Select(e => e.EnrollmentId).ToList();

        var sessions = await context.CatechismSessions
            .AsNoTracking()
            .Where(s => classIds.Contains(s.CatechismClassId))
            .Select(s => new { s.Id, s.Title, s.DateTime, s.CatechismClassId })
            .ToListAsync(cancellationToken);

        var attendances = await context.CatechismSessionAttendances
            .AsNoTracking()
            .Where(a => scopedIds.Contains(a.CatechismClassEucharisticYouthId))
            .Select(a => new { a.CatechismSessionId, a.CatechismClassEucharisticYouthId, a.AttendanceStatus })
            .ToListAsync(cancellationToken);

        var statusLookup = new Dictionary<(Guid SessionId, Guid EnrollmentId), AttendanceStatus>();
        foreach (var attendance in attendances)
        {
            statusLookup.TryAdd((attendance.CatechismSessionId, attendance.CatechismClassEucharisticYouthId), attendance.AttendanceStatus);
        }

        var exams = await context.Exams
            .AsNoTracking()
            .Where(e => classIds.Contains(e.CatechismClassId))
            .Select(e => new { e.Id, e.Title, e.ExamDate, e.CatechismClassId })
            .ToListAsync(cancellationToken);

        var results = await context.ExamResults
            .AsNoTracking()
            .Where(r => scopedIds.Contains(r.CatechismClassEucharisticYouthId))
            .Select(r => new { r.ExamId, r.CatechismClassEucharisticYouthId, r.Score, r.Remark })
            .ToListAsync(cancellationToken);

        var resultLookup = new Dictionary<(Guid ExamId, Guid EnrollmentId), (double? Score, string? Remark)>();
        foreach (var result in results)
        {
            resultLookup.TryAdd((result.ExamId, result.CatechismClassEucharisticYouthId), (result.Score, result.Remark));
        }

        var now = DateTimeOffset.UtcNow;
        var today = DateOnly.FromDateTime(now.UtcDateTime);
        var attendanceDetails = new List<YouthAttendanceReportItemDto>();
        var examDetails = new List<YouthExamReportItemDto>();

        foreach (var enrollment in scoped)
        {
            foreach (var session in sessions.Where(s => s.CatechismClassId == enrollment.ClassId))
            {
                var status = statusLookup.GetValueOrDefault((session.Id, enrollment.EnrollmentId), AttendanceStatus.None);
                var isCounted = IsCounted(session.DateTime, enrollment.EnrolledAt, enrollment.LeftAt, now);
                if (!isCounted && status == AttendanceStatus.None)
                {
                    continue;
                }

                attendanceDetails.Add(new YouthAttendanceReportItemDto
                {
                    SessionId = session.Id,
                    SessionTitle = session.Title,
                    SessionDateTime = session.DateTime,
                    ClassId = enrollment.ClassId,
                    ClassName = enrollment.ClassName,
                    Status = status,
                    IsCounted = isCounted
                });
            }

            foreach (var exam in exams.Where(e => e.CatechismClassId == enrollment.ClassId))
            {
                var hasResult = resultLookup.TryGetValue((exam.Id, enrollment.EnrollmentId), out var result);
                if (!hasResult && (exam.ExamDate > today || !IsWithinEnrollment(exam.ExamDate, enrollment.EnrolledAt, enrollment.LeftAt)))
                {
                    continue;
                }

                examDetails.Add(new YouthExamReportItemDto
                {
                    ExamId = exam.Id,
                    ExamTitle = exam.Title,
                    ExamDate = exam.ExamDate,
                    ClassId = enrollment.ClassId,
                    ClassName = enrollment.ClassName,
                    Score = result.Score,
                    Remark = result.Remark
                });
            }
        }

        var scoredValues = examDetails.Where(e => e.Score.HasValue).Select(e => e.Score!.Value).ToList();

        return new YouthReportDto
        {
            YouthId = youth.Id,
            YouthCode = youth.Code,
            YouthFullName = youth.FullName,
            CatechismYearId = catechismYearId,
            SelectedEnrollmentId = enrollmentId,
            Enrollments = enrollments,
            Attendance = CreateAttendanceSummary(attendanceDetails.Where(a => a.IsCounted).Select(a => a.Status)),
            Exams = new YouthExamSummaryDto
            {
                Total = examDetails.Count,
                Scored = scoredValues.Count,
                AverageScore = Average(scoredValues)
            },
            AttendanceDetails = attendanceDetails.OrderByDescending(a => a.SessionDateTime).ToList(),
            ExamDetails = examDetails.OrderByDescending(e => e.ExamDate).ToList()
        };
    }

    private static bool IsCounted(DateTimeOffset sessionDateTime, DateTimeOffset enrolledAt, DateTimeOffset? leftAt, DateTimeOffset now) =>
        sessionDateTime <= now && IsWithinEnrollment(DateOnly.FromDateTime(sessionDateTime.UtcDateTime), enrolledAt, leftAt);

    // Compared by calendar date so a session on the enrollment day still counts.
    private static bool IsWithinEnrollment(DateOnly date, DateTimeOffset enrolledAt, DateTimeOffset? leftAt) =>
        date >= DateOnly.FromDateTime(enrolledAt.UtcDateTime)
        && (!leftAt.HasValue || date <= DateOnly.FromDateTime(leftAt.Value.UtcDateTime));

    private static YouthAttendanceSummaryDto CreateAttendanceSummary(IEnumerable<AttendanceStatus> countedStatuses)
    {
        var statuses = countedStatuses.ToList();
        var present = statuses.Count(s => s == AttendanceStatus.Present);

        return new YouthAttendanceSummaryDto
        {
            SessionsHeld = statuses.Count,
            Present = present,
            Absent = statuses.Count(s => s == AttendanceStatus.Absent),
            Excused = statuses.Count(s => s == AttendanceStatus.Excused),
            Unmarked = statuses.Count(s => s == AttendanceStatus.None),
            AttendanceRate = statuses.Count == 0 ? null : Math.Round((decimal)present / statuses.Count * 100, 2)
        };
    }

    private static double? Average(List<double> values) =>
        values.Count == 0 ? null : Math.Round(values.Average(), 2);
}
