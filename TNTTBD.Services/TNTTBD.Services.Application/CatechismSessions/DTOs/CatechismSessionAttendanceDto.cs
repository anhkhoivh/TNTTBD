using TNTTBD.Services.Domain.Enums;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.CatechismSessions.DTOs;

public class CatechismSessionAttendanceDto
{
    public Guid? Id { get; set; }
    public Guid CatechismClassEucharisticYouthId { get; set; }
    public string? YouthCode { get; set; }
    public FullName? YouthFullName { get; set; }
    public AttendanceStatus AttendanceStatus { get; set; }
}
