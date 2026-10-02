using MediatR;
using TNTTBD.Services.Application.Reports.DTOs;

namespace TNTTBD.Services.Application.Reports.Queries;

public record ExportClassReportExcelQuery(
    Guid ClassId,
    IReadOnlyList<Guid> AttendanceEnrollmentIds,
    IReadOnlyList<Guid> ExamEnrollmentIds,
    string Language) : IRequest<ClassReportExcelFileDto?>;