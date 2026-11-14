namespace TNTTBD.Services.Application.MilestoneEvents.DTOs;

public class MilestoneEventExcelFileDto
{
    public byte[] Content { get; set; } = [];
    public string FileName { get; set; } = string.Empty;
}
