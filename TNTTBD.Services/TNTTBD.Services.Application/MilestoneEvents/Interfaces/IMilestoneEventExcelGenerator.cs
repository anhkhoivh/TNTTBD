using TNTTBD.Services.Application.MilestoneEvents.DTOs;

namespace TNTTBD.Services.Application.MilestoneEvents.Interfaces;

public interface IMilestoneEventExcelGenerator
{
    byte[] Generate(MilestoneEventExportDto data, string language);
}
