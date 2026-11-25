using TNTTBD.Services.Application.CatechismSessions.DTOs;

namespace TNTTBD.Services.Application.CatechismSessions.Commands;
public record MarkAttendanceCommand(
    Guid SessionId,
    AttendanceItemDto AttendanceItem,
    Guid MarkedByCatechistId
);