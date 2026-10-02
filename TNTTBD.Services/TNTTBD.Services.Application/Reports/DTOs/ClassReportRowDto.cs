using TNTTBD.Services.Domain.Enums;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.Reports.DTOs;

public class ClassReportRowDto
{
    public Guid EnrollmentId { get; set; }
    public Guid YouthId { get; set; }
    public string? YouthCode { get; set; }
    public FullName? YouthFullName { get; set; }
    public EnrollmentStatus EnrollmentStatus { get; set; }
    public DateTimeOffset EnrolledAt { get; set; }
    public DateTimeOffset? LeftAt { get; set; }
    public List<ClassReportAttendanceCellDto> Attendances { get; set; } = [];
    public List<ClassReportScoreCellDto> Scores { get; set; } = [];
    public int Counted { get; set; }
    public int Present { get; set; }
    public int Absent { get; set; }
    public int Excused { get; set; }
    public int Unmarked { get; set; }
    public decimal? AttendanceRate { get; set; }
    public int Scored { get; set; }
    public double? AverageScore { get; set; }
}
