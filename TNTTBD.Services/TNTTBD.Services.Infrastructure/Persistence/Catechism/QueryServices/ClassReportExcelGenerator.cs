using ClosedXML.Excel;
using TNTTBD.Services.Application.Reports.DTOs;
using TNTTBD.Services.Application.Reports.Interfaces;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.QueryServices;

public class ClassReportExcelGenerator : IClassReportExcelGenerator
{
    public byte[] Generate(
        ClassReportDto report,
        IReadOnlyList<ClassReportRowDto> attendanceRows,
        IReadOnlyList<ClassReportRowDto> examRows,
        string language)
    {
        using var workbook = new XLWorkbook();
        var culture = language == "vi" ? "vi-VN" : "en-US";
        var attendanceSheet = workbook.Worksheets.Add(Text(language, "report.attendance"));
        var attendanceHeaders = new List<string>
        {
            Text(language, "common.index"),
            Text(language, "youth.code"),
            Text(language, "youth.fullName"),
            Text(language, "report.enrollmentStatus")
        };
        attendanceHeaders.AddRange(report.Sessions.Select(session =>
            $"{session.Title} ({session.DateTime.ToString("d", System.Globalization.CultureInfo.GetCultureInfo(culture))})"));
        attendanceHeaders.AddRange(
        [
            Text(language, "attendance.present"),
            Text(language, "attendance.excused"),
            Text(language, "attendance.absent"),
            Text(language, "attendance.none"),
            Text(language, "report.rate")
        ]);

        WriteHeader(attendanceSheet, attendanceHeaders);
        for (var rowIndex = 0; rowIndex < attendanceRows.Count; rowIndex++)
        {
            var row = attendanceRows[rowIndex];
            var excelRow = rowIndex + 2;
            attendanceSheet.Cell(excelRow, 1).Value = rowIndex + 1;
            attendanceSheet.Cell(excelRow, 2).Value = row.YouthCode ?? string.Empty;
            attendanceSheet.Cell(excelRow, 3).Value = row.YouthFullName?.ToString() ?? string.Empty;
            attendanceSheet.Cell(excelRow, 4).Value = Text(language, $"enrollment.status.{row.EnrollmentStatus}");

            for (var sessionIndex = 0; sessionIndex < report.Sessions.Count; sessionIndex++)
            {
                var cell = row.Attendances[sessionIndex];
                var status = Text(language, $"attendance.{cell.Status}");
                attendanceSheet.Cell(excelRow, 5 + sessionIndex).Value = cell.IsCounted
                    ? status
                    : $"{status} ({Text(language, "report.notCounted")})";
            }

            var summaryColumn = 5 + report.Sessions.Count;
            attendanceSheet.Cell(excelRow, summaryColumn).Value = row.Present;
            attendanceSheet.Cell(excelRow, summaryColumn + 1).Value = row.Excused;
            attendanceSheet.Cell(excelRow, summaryColumn + 2).Value = row.Absent;
            attendanceSheet.Cell(excelRow, summaryColumn + 3).Value = row.Unmarked;
            if (row.AttendanceRate.HasValue)
            {
                attendanceSheet.Cell(excelRow, summaryColumn + 4).Value = (double)(row.AttendanceRate.Value / 100m);
                attendanceSheet.Cell(excelRow, summaryColumn + 4).Style.NumberFormat.Format = "0.0%";
            }
        }

        var attendanceTotalRow = attendanceRows.Count + 2;
        attendanceSheet.Cell(attendanceTotalRow, 1).Value = Text(language, "report.total");
        for (var sessionIndex = 0; sessionIndex < report.Sessions.Count; sessionIndex++)
        {
            if (!report.Sessions[sessionIndex].IsHeld)
            {
                continue;
            }

            var counted = attendanceRows.Count(row => row.Attendances[sessionIndex].IsCounted);
            var present = attendanceRows.Count(row =>
                row.Attendances[sessionIndex].IsCounted
                && row.Attendances[sessionIndex].Status == AttendanceStatus.Present);
            attendanceSheet.Cell(attendanceTotalRow, 5 + sessionIndex).Value = $"{present}/{counted}";
        }

        var attendanceSummaryColumn = 5 + report.Sessions.Count;
        attendanceSheet.Cell(attendanceTotalRow, attendanceSummaryColumn).Value = attendanceRows.Sum(row => row.Present);
        attendanceSheet.Cell(attendanceTotalRow, attendanceSummaryColumn + 1).Value = attendanceRows.Sum(row => row.Excused);
        attendanceSheet.Cell(attendanceTotalRow, attendanceSummaryColumn + 2).Value = attendanceRows.Sum(row => row.Absent);
        attendanceSheet.Cell(attendanceTotalRow, attendanceSummaryColumn + 3).Value = attendanceRows.Sum(row => row.Unmarked);
        var countedSessions = attendanceRows.Sum(row => row.Counted);
        if (countedSessions > 0)
        {
            attendanceSheet.Cell(attendanceTotalRow, attendanceSummaryColumn + 4).Value =
                (double)attendanceRows.Sum(row => row.Present) / countedSessions;
            attendanceSheet.Cell(attendanceTotalRow, attendanceSummaryColumn + 4).Style.NumberFormat.Format = "0.0%";
        }

        FinishSheet(attendanceSheet, attendanceTotalRow, attendanceHeaders.Count);

        var examSheet = workbook.Worksheets.Add(Text(language, "report.exams"));
        var examHeaders = new List<string>
        {
            Text(language, "common.index"),
            Text(language, "youth.code"),
            Text(language, "youth.fullName"),
            Text(language, "report.enrollmentStatus")
        };
        examHeaders.AddRange(report.Exams.Select(exam =>
            $"{exam.Title} ({exam.ExamDate.ToString("d", System.Globalization.CultureInfo.GetCultureInfo(culture))})"));
        examHeaders.Add(Text(language, "report.scored"));
        examHeaders.Add(Text(language, "report.average"));
        WriteHeader(examSheet, examHeaders);

        for (var rowIndex = 0; rowIndex < examRows.Count; rowIndex++)
        {
            var row = examRows[rowIndex];
            var excelRow = rowIndex + 2;
            examSheet.Cell(excelRow, 1).Value = rowIndex + 1;
            examSheet.Cell(excelRow, 2).Value = row.YouthCode ?? string.Empty;
            examSheet.Cell(excelRow, 3).Value = row.YouthFullName?.ToString() ?? string.Empty;
            examSheet.Cell(excelRow, 4).Value = Text(language, $"enrollment.status.{row.EnrollmentStatus}");

            for (var examIndex = 0; examIndex < report.Exams.Count; examIndex++)
            {
                var score = row.Scores[examIndex];
                if (score.Score.HasValue)
                {
                    examSheet.Cell(excelRow, 5 + examIndex).Value = score.Score.Value;
                }
                if (!string.IsNullOrWhiteSpace(score.Remark))
                {
                    examSheet.Cell(excelRow, 5 + examIndex).GetComment().AddText(score.Remark);
                }
            }

            examSheet.Cell(excelRow, 5 + report.Exams.Count).Value = $"{row.Scored}/{report.Exams.Count}";
            if (row.AverageScore.HasValue)
            {
                examSheet.Cell(excelRow, 6 + report.Exams.Count).Value = row.AverageScore.Value;
                examSheet.Cell(excelRow, 6 + report.Exams.Count).Style.NumberFormat.Format = "0.00";
            }
        }

        var examTotalRow = examRows.Count + 2;
        examSheet.Cell(examTotalRow, 1).Value = Text(language, "report.average");
        for (var examIndex = 0; examIndex < report.Exams.Count; examIndex++)
        {
            var scores = examRows
                .Select(row => row.Scores[examIndex].Score)
                .Where(score => score.HasValue)
                .Select(score => score!.Value)
                .ToList();
            if (scores.Count > 0)
            {
                examSheet.Cell(examTotalRow, 5 + examIndex).Value = scores.Average();
                examSheet.Cell(examTotalRow, 5 + examIndex).Style.NumberFormat.Format = "0.00";
            }
        }

        examSheet.Cell(examTotalRow, 5 + report.Exams.Count).Value = examRows.Sum(row => row.Scored);
        var averages = examRows.Where(row => row.AverageScore.HasValue).Select(row => row.AverageScore!.Value).ToList();
        if (averages.Count > 0)
        {
            examSheet.Cell(examTotalRow, 6 + report.Exams.Count).Value = averages.Average();
            examSheet.Cell(examTotalRow, 6 + report.Exams.Count).Style.NumberFormat.Format = "0.00";
        }

        FinishSheet(examSheet, examTotalRow, examHeaders.Count);

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    private static void WriteHeader(IXLWorksheet worksheet, IReadOnlyList<string> headers)
    {
        for (var column = 0; column < headers.Count; column++)
        {
            worksheet.Cell(1, column + 1).Value = headers[column];
        }

        worksheet.Row(1).Style.Font.Bold = true;
        worksheet.Row(1).Style.Alignment.WrapText = true;
        worksheet.SheetView.FreezeRows(1);
        worksheet.SheetView.FreezeColumns(4);
    }

    private static void FinishSheet(IXLWorksheet worksheet, int totalRow, int columnCount)
    {
        worksheet.Row(totalRow).Style.Font.Bold = true;
        worksheet.Range(1, 1, totalRow, columnCount).SetAutoFilter();
        worksheet.Columns(1, columnCount).AdjustToContents(8, 40);
        worksheet.Column(3).Width = Math.Max(worksheet.Column(3).Width, 26);
    }

    private static string Text(string language, string key)
    {
        var normalizedLanguage = language == "vi" ? "vi" : "en";
        return (normalizedLanguage, key) switch
        {
            ("en", "common.index") => "Index",
            ("en", "youth.code") => "Code",
            ("en", "youth.fullName") => "Full name",
            ("en", "report.enrollmentStatus") => "Enrollment status",
            ("en", "attendance.present") => "Present",
            ("en", "attendance.excused") => "Excused",
            ("en", "attendance.absent") => "Absent",
            ("en", "attendance.none") => "Not marked",
            ("en", "report.rate") => "Attendance rate",
            ("en", "report.total") => "Total",
            ("en", "report.scored") => "Scored",
            ("en", "report.average") => "Average",
            ("en", "report.notCounted") => "Outside enrollment period",
            ("en", "report.attendance") => "Attendance",
            ("en", "report.exams") => "Exam Scores",
            ("en", "attendance.Present") => "Present",
            ("en", "attendance.Excused") => "Excused",
            ("en", "attendance.Absent") => "Absent",
            ("en", "attendance.None") => "Not marked",
            ("vi", "common.index") => "STT",
            ("vi", "youth.code") => "Mã số thiếu nhi",
            ("vi", "youth.fullName") => "Họ và tên",
            ("vi", "report.enrollmentStatus") => "Tình trạng học",
            ("vi", "attendance.present") => "Hiện diện",
            ("vi", "attendance.excused") => "Vắng có phép",
            ("vi", "attendance.absent") => "Vắng không phép",
            ("vi", "attendance.none") => "Chưa điểm danh",
            ("vi", "report.rate") => "Tỉ lệ hiện diện",
            ("vi", "report.total") => "Tổng",
            ("vi", "report.scored") => "Đã có điểm",
            ("vi", "report.average") => "Điểm trung bình",
            ("vi", "report.notCounted") => "Ngoài thời gian học lớp",
            ("vi", "report.attendance") => "Điểm danh",
            ("vi", "report.exams") => "Điểm kiểm tra",
            ("vi", "attendance.Present") => "Hiện diện",
            ("vi", "attendance.Excused") => "Vắng có phép",
            ("vi", "attendance.Absent") => "Vắng không phép",
            ("vi", "attendance.None") => "Chưa điểm danh",
            _ when key.StartsWith("enrollment.status.", StringComparison.Ordinal) => EnrollmentStatusText(key, normalizedLanguage),
            _ => key
        };
    }

    private static string EnrollmentStatusText(string key, string language)
    {
        var status = key["enrollment.status.".Length..];
        return (language, status) switch
        {
            ("vi", "Pending") => "Chờ duyệt",
            ("vi", "Active") => "Đang học",
            ("vi", "Transferred") => "Chuyển lớp",
            ("vi", "Suspended") => "Tạm nghỉ",
            ("vi", "Completed") => "Hoàn thành",
            ("en", "Pending") => "Pending",
            ("en", "Active") => "Active",
            ("en", "Transferred") => "Transferred",
            ("en", "Suspended") => "Suspended",
            ("en", "Completed") => "Completed",
            _ => status
        };
    }
}