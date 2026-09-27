namespace TNTTBD.Services.Application.Reports.DTOs;

public class ClassSessionReportItemDto
{
    public Guid SessionId { get; set; }
    public string? Title { get; set; }
    public DateTimeOffset DateTime { get; set; }
    public int TotalAttendance { get; set; }
    public int Present { get; set; }
    public int Absent { get; set; }
    public int Excused { get; set; }
}