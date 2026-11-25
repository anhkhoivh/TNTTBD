using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Reports.DTOs;
using TNTTBD.Services.Application.Reports.Interfaces;
using TNTTBD.Services.Application.Reports.Queries;

namespace TNTTBD.Services.Application.Reports.Handlers;

public class ExportClassReportExcelHandler(
    IReportQueryService reportQueryService,
    IClassReportExcelGenerator excelGenerator,
    IAuthorizationContext authorizationContext)
    : IRequestHandler<ExportClassReportExcelQuery, ClassReportExcelFileDto?>
{
    public async Task<ClassReportExcelFileDto?> Handle(
        ExportClassReportExcelQuery query,
        CancellationToken cancellationToken)
    {
        if (!await authorizationContext.IsAdminOrCatechistAsync(cancellationToken)
            || !await authorizationContext.CanAccessClassAsync(query.ClassId, cancellationToken))
        {
            return null;
        }

        var report = await reportQueryService.GetClassReportAsync(query.ClassId, cancellationToken);
        if (report == null)
        {
            return null;
        }

        var rowsById = report.Rows.ToDictionary(row => row.EnrollmentId);
        var attendanceRows = SelectRows(query.AttendanceEnrollmentIds, rowsById);
        var examRows = SelectRows(query.ExamEnrollmentIds, rowsById);
        var language = query.Language.StartsWith("vi", StringComparison.OrdinalIgnoreCase) ? "vi" : "en";
        var bytes = excelGenerator.Generate(report, attendanceRows, examRows, language);
        var fileStem = string.Join('-', new[] { report.ClassCode, report.ClassName }
            .Where(value => !string.IsNullOrWhiteSpace(value)));
        if (string.IsNullOrWhiteSpace(fileStem))
        {
            fileStem = report.ClassId.ToString();
        }

        const string windowsInvalidCharacters = "<>:\"/\\|?*";
        fileStem = new string(fileStem
            .Select(character => char.IsControl(character) || windowsInvalidCharacters.Contains(character) ? '-' : character)
            .ToArray());

        return new ClassReportExcelFileDto
        {
            Content = bytes,
            FileName = $"{fileStem}-report.xlsx"
        };
    }

    private static List<ClassReportRowDto> SelectRows(
        IEnumerable<Guid> enrollmentIds,
        IReadOnlyDictionary<Guid, ClassReportRowDto> rowsById)
    {
        return enrollmentIds
            .Distinct()
            .Where(rowsById.ContainsKey)
            .Select(id => rowsById[id])
            .ToList();
    }
}