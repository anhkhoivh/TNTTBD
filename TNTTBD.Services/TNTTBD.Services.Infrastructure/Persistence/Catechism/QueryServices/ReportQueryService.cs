using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.Reports.DTOs;
using TNTTBD.Services.Application.Reports.Interfaces;
using TNTTBD.Services.Domain.Enums;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

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

        var sessions = await context.CatechismSessions
            .AsNoTracking()
            .Where(s => s.CatechismClassId == classId)
            .OrderByDescending(s => s.DateTime)
            .Select(s => new ClassSessionReportItemDto
            {
                SessionId = s.Id,
                Title = s.Title,
                DateTime = s.DateTime,
                TotalAttendance = s.EucharisticYouthAttendances.Count,
                Present = s.EucharisticYouthAttendances.Count(a => a.AttendanceStatus == AttendanceStatus.Present),
                Absent = s.EucharisticYouthAttendances.Count(a => a.AttendanceStatus == AttendanceStatus.Absent),
                Excused = s.EucharisticYouthAttendances.Count(a => a.AttendanceStatus == AttendanceStatus.Excused)
            })
            .ToListAsync(cancellationToken);

        var exams = await context.Exams
            .AsNoTracking()
            .Where(e => e.CatechismClassId == classId)
            .OrderByDescending(e => e.ExamDate)
            .Select(e => new ClassExamReportItemDto
            {
                ExamId = e.Id,
                Title = e.Title,
                ExamDate = e.ExamDate,
                TotalResults = e.ExamResults.Count,
                Scored = e.ExamResults.Count(r => r.Score.HasValue),
                AverageScore = e.ExamResults.Where(r => r.Score.HasValue).Select(r => r.Score).Average()
            })
            .ToListAsync(cancellationToken);

        return new ClassReportDto
        {
            ClassId = classInfo.Id,
            ClassCode = classInfo.ClassCode,
            ClassName = classInfo.ClassName,
            CatechismYearId = classInfo.CatechismYearId,
            CatechismYearName = classInfo.YearTheme,
            Sessions = sessions,
            Exams = exams
        };
    }

    public async Task<YouthReportDto?> GetYouthReportAsync(
        Guid youthId,
        Guid? catechismYearId,
        int page,
        int? pageSize,
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

        if (!catechismYearId.HasValue)
        {
            catechismYearId = await context.CatechismYears
                .AsNoTracking()
                .OrderByDescending(y => y.StartDate)
                .Select(y => (Guid?)y.Id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        var enrollments = context.CatechismClassEucharisticYouths
            .AsNoTracking()
            .Where(e => e.EucharisticYouthId == youthId);

        if (catechismYearId.HasValue)
        {
            enrollments = enrollments.Where(e => e.CatechismClass.CatechismYearId == catechismYearId.Value);
        }

        var enrollmentIds = enrollments.Select(e => e.Id);

        var classes = await enrollments
            .OrderByDescending(e => e.EnrolledAt)
            .Select(e => new YouthClassReportDto
            {
                ClassId = e.CatechismClassId,
                ClassCode = e.CatechismClass.ClassCode,
                ClassName = e.CatechismClass.ClassName,
                CatechismYearId = e.CatechismClass.CatechismYearId,
                CatechismYearName = e.CatechismClass.CatechismYear.Theme,
                EnrollmentStatus = e.Status,
                EnrolledAt = e.EnrolledAt,
                LeftAt = e.LeftAt
            })
            .ToListAsync(cancellationToken);

        var attendanceQuery = context.CatechismSessionAttendances
            .AsNoTracking()
            .Where(a => enrollmentIds.Contains(a.CatechismClassEucharisticYouthId));

        var attendanceTotal = await attendanceQuery.CountAsync(cancellationToken);
        var attendanceCounts = await attendanceQuery
            .GroupBy(a => a.AttendanceStatus)
            .Select(group => new { Status = group.Key, Count = group.Count() })
            .ToListAsync(cancellationToken);

        int CountAttendance(AttendanceStatus status) =>
            attendanceCounts.FirstOrDefault(x => x.Status == status)?.Count ?? 0;

        var present = CountAttendance(AttendanceStatus.Present);
        var absent = CountAttendance(AttendanceStatus.Absent);
        var excused = CountAttendance(AttendanceStatus.Excused);

        var examQuery = context.ExamResults
            .AsNoTracking()
            .Where(r => enrollmentIds.Contains(r.CatechismClassEucharisticYouthId));

        var examTotal = await examQuery.CountAsync(cancellationToken);
        var scored = await examQuery.CountAsync(r => r.Score.HasValue, cancellationToken);
        var averageScore = await examQuery
            .Where(r => r.Score.HasValue)
            .Select(r => r.Score)
            .AverageAsync(cancellationToken);

        var attendanceDetails = await CreateAttendanceDetailsAsync(attendanceQuery, page, pageSize, cancellationToken);
        var examDetails = await CreateExamDetailsAsync(examQuery, page, pageSize, cancellationToken);

        return new YouthReportDto
        {
            YouthId = youth.Id,
            YouthCode = youth.Code,
            YouthFullName = youth.FullName,
            Classes = classes,
            Attendance = new YouthAttendanceSummaryDto
            {
                Total = attendanceTotal,
                Present = present,
                Absent = absent,
                Excused = excused,
                AttendanceRate = attendanceTotal == 0 ? 0 : Math.Round((decimal)present / attendanceTotal * 100, 2)
            },
            Exams = new YouthExamSummaryDto
            {
                Total = examTotal,
                Scored = scored,
                AverageScore = averageScore
            },
            AttendanceDetails = attendanceDetails,
            ExamDetails = examDetails
        };
    }

    private static async Task<PaginatedResult<YouthAttendanceReportItemDto>> CreateAttendanceDetailsAsync(
        IQueryable<CatechismSessionAttendanceEntity> query,
        int page,
        int? pageSize,
        CancellationToken cancellationToken)
    {
        var total = await query.CountAsync(cancellationToken);
        IQueryable<CatechismSessionAttendanceEntity> pagedQuery = query.OrderByDescending(a => a.CatechismSession.DateTime);

        if (pageSize.HasValue)
        {
            pagedQuery = pagedQuery.Skip((page - 1) * pageSize.Value).Take(pageSize.Value);
        }

        var items = await pagedQuery
            .Select(a => new YouthAttendanceReportItemDto
            {
                SessionId = a.CatechismSessionId,
                SessionTitle = a.CatechismSession.Title,
                SessionDateTime = a.CatechismSession.DateTime,
                ClassId = a.CatechismSession.CatechismClassId,
                ClassName = a.CatechismSession.CatechismClass.ClassName,
                Status = a.AttendanceStatus
            })
            .ToListAsync(cancellationToken);

        return new PaginatedResult<YouthAttendanceReportItemDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize ?? total
        };
    }

    private static async Task<PaginatedResult<YouthExamReportItemDto>> CreateExamDetailsAsync(
        IQueryable<ExamResultEntity> query,
        int page,
        int? pageSize,
        CancellationToken cancellationToken)
    {
        var total = await query.CountAsync(cancellationToken);
        IQueryable<ExamResultEntity> pagedQuery = query.OrderByDescending(r => r.Exam.ExamDate);

        if (pageSize.HasValue)
        {
            pagedQuery = pagedQuery.Skip((page - 1) * pageSize.Value).Take(pageSize.Value);
        }

        var items = await pagedQuery
            .Select(r => new YouthExamReportItemDto
            {
                ExamId = r.ExamId,
                ExamTitle = r.Exam.Title,
                ExamDate = r.Exam.ExamDate,
                ClassId = r.Exam.CatechismClassId,
                ClassName = r.Exam.CatechismClass.ClassName,
                Score = r.Score,
                Remark = r.Remark
            })
            .ToListAsync(cancellationToken);

        return new PaginatedResult<YouthExamReportItemDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize ?? total
        };
    }
}