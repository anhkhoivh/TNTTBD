using ClosedXML.Excel;
using TNTTBD.Services.Application.MilestoneEvents.DTOs;
using TNTTBD.Services.Application.MilestoneEvents.Interfaces;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Enums;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.QueryServices;

/// <summary>
/// Excel list of a milestone event's participants, ordered by name.
/// Strings are written as text values (never formulas), so user data cannot inject formulas.
/// </summary>
public class MilestoneEventExcelGenerator : IMilestoneEventExcelGenerator
{
    private const string DateFormat = "dd/MM/yyyy";

    public byte[] Generate(MilestoneEventExportDto data, string language)
    {
        using var workbook = new XLWorkbook();
        IXLWorksheet sheet = workbook.Worksheets.Add(Text(language, "sheet"));
        bool withSponsor = SpiritualMilestone.SupportsSponsor(data.Type);

        List<string> headers =
        [
            Text(language, "code"),
            Text(language, "christName"),
            Text(language, "familyMiddleName"),
            Text(language, "givenName"),
            Text(language, "dateOfBirth"),
            Text(language, "baptismDate"),
            Text(language, "address"),
            Text(language, "parents")
        ];
        if (withSponsor)
        {
            headers.Add(Text(language, data.Type == MilestoneType.Baptism ? "sponsor.baptism" : "sponsor.confirmation"));
        }
        headers.Add(Text(language, "celebrant"));

        for (int column = 0; column < headers.Count; column++)
        {
            sheet.Cell(1, column + 1).Value = headers[column];
        }
        sheet.Row(1).Style.Font.Bold = true;
        sheet.Row(1).Style.Alignment.WrapText = true;
        sheet.SheetView.FreezeRows(1);

        for (int i = 0; i < data.Rows.Count; i++)
        {
            MilestoneEventExportRowDto row = data.Rows[i];
            int r = i + 2;
            SetText(sheet.Cell(r, 1), row.DocumentCode);
            SetText(sheet.Cell(r, 2), row.FullName.ChristName);
            SetText(sheet.Cell(r, 3), string.Join(" ", new[] { row.FullName.FamilyName, row.FullName.MiddleName }.Where(x => !string.IsNullOrWhiteSpace(x))));
            SetText(sheet.Cell(r, 4), row.FullName.GivenName);
            SetDate(sheet.Cell(r, 5), row.DateOfBirth);
            SetDate(sheet.Cell(r, 6), row.BaptismDate);
            SetText(sheet.Cell(r, 7), row.Address);

            IXLCell parentsCell = sheet.Cell(r, 8);
            SetText(parentsCell, string.Join("\n", row.Parents.Select(p => $"{Text(language, p.Relation == ContactRelationType.Father ? "father" : "mother")}: {FormatName(p.FullName)}")));
            parentsCell.Style.Alignment.WrapText = true;

            int column = 9;
            if (withSponsor)
            {
                SetText(sheet.Cell(r, column++), row.SponsorFullName == null ? null : FormatName(row.SponsorFullName));
            }
            SetText(sheet.Cell(r, column), row.Celebrant);
        }

        int lastRow = Math.Max(1, data.Rows.Count + 1);
        sheet.Range(1, 1, lastRow, headers.Count).SetAutoFilter();
        sheet.Columns(1, headers.Count).AdjustToContents(8, 50);
        sheet.Column(1).Style.NumberFormat.Format = "@";
        sheet.Column(8).Width = Math.Max(sheet.Column(8).Width, 40);
        sheet.Range(1, 1, lastRow, headers.Count).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    private static void SetText(IXLCell cell, string? value)
    {
        cell.Value = value ?? string.Empty;
        cell.Style.NumberFormat.Format = "@";
    }

    private static void SetDate(IXLCell cell, DateOnly? value)
    {
        if (value.HasValue)
        {
            cell.Value = value.Value.ToDateTime(TimeOnly.MinValue);
            cell.Style.DateFormat.Format = DateFormat;
        }
    }

    private static string FormatName(FullName name) => name.ToString();

    private static string Text(string language, string key)
    {
        bool vi = language == "vi";
        return key switch
        {
            "sheet" => vi ? "Danh sách" : "List",
            "code" => vi ? "Mã số" : "Milestone Code",
            "christName" => vi ? "Tên Thánh" : "Christ Name",
            "familyMiddleName" => vi ? "Họ và tên đệm" : "Family and Middle Name",
            "givenName" => vi ? "Tên" : "Given Name",
            "dateOfBirth" => vi ? "Ngày sinh" : "Date of Birth",
            "baptismDate" => vi ? "Ngày Rửa Tội" : "Date of Baptism",
            "address" => vi ? "Địa chỉ" : "Address",
            "parents" => vi ? "Họ tên Cha + Mẹ" : "Father + Mother Full Name",
            "father" => vi ? "Cha" : "Father",
            "mother" => vi ? "Mẹ" : "Mother",
            "sponsor.baptism" => vi ? "Họ tên Cha/Mẹ đỡ đầu" : "Godfather/Godmother Full Name",
            "sponsor.confirmation" => vi ? "Họ tên Người đỡ đầu" : "Godfather/Godmother Full Name",
            "celebrant" => vi ? "Chủ sự" : "Celebrant",
            _ => key
        };
    }
}
