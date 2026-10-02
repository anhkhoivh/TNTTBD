namespace TNTTBD.Services.Application.Reports.DTOs;

public class YouthAttendanceSummaryDto
{
    public int SessionsHeld { get; set; }
    public int Present { get; set; }
    public int Absent { get; set; }
    public int Excused { get; set; }
    public int Unmarked { get; set; }
    public decimal? AttendanceRate { get; set; }
}