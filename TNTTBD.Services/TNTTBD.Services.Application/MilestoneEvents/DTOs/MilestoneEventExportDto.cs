using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.MilestoneEvents.DTOs;

public class MilestoneEventExportDto
{
    public string Name { get; set; } = string.Empty;
    public MilestoneType Type { get; set; }
    public DateOnly Date { get; set; }
    public List<MilestoneEventExportRowDto> Rows { get; set; } = [];
}
