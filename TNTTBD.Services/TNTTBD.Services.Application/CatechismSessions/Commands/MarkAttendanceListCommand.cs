using TNTTBD.Services.Application.CatechismSessions.DTOs;

namespace TNTTBD.Services.Application.CatechismSessions.Commands;
public record MarkAttendanceListCommand(
    Guid SessionId,
    List<AttendanceItemDto> AttendanceItems,
    Guid MarkedByCatechistId
);