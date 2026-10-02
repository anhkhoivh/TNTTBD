namespace TNTTBD.Services.Application.Reports.DTOs;

public class ClassReportSessionDto
{
    public Guid SessionId { get; set; }
    public string? Title { get; set; }
    public DateTimeOffset DateTime { get; set; }
    public bool IsHeld { get; set; }
}
