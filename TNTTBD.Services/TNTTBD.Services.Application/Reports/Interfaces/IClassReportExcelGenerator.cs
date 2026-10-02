using TNTTBD.Services.Application.Reports.DTOs;

namespace TNTTBD.Services.Application.Reports.Interfaces;

public interface IClassReportExcelGenerator
{
    byte[] Generate(
        ClassReportDto report,
        IReadOnlyList<ClassReportRowDto> attendanceRows,
        IReadOnlyList<ClassReportRowDto> examRows,
        string language);
}