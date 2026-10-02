namespace TNTTBD.Services.Application.Reports.DTOs;

public class ClassReportExcelFileDto
{
    public byte[] Content { get; set; } = [];
    public string FileName { get; set; } = string.Empty;
}