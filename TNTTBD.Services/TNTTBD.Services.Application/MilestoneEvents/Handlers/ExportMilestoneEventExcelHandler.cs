using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Common.Exceptions;
using TNTTBD.Services.Application.MilestoneEvents.DTOs;
using TNTTBD.Services.Application.MilestoneEvents.Interfaces;
using TNTTBD.Services.Application.MilestoneEvents.Queries;

namespace TNTTBD.Services.Application.MilestoneEvents.Handlers;

public class ExportMilestoneEventExcelHandler(
    IMilestoneEventQueryService queryService,
    IMilestoneEventExcelGenerator excelGenerator,
    IAuthorizationContext auth) : IRequestHandler<ExportMilestoneEventExcelQuery, MilestoneEventExcelFileDto?>
{
    public async Task<MilestoneEventExcelFileDto?> Handle(ExportMilestoneEventExcelQuery query, CancellationToken cancellationToken)
    {
        if (!await auth.IsAdminOrCatechistAsync(cancellationToken))
        {
            throw new ForbiddenException();
        }
        MilestoneEventExportDto? data = await queryService.GetExportDataAsync(query.Id, cancellationToken);
        if (data == null)
        {
            return null;
        }

        string language = (query.Language ?? string.Empty).StartsWith("vi", StringComparison.OrdinalIgnoreCase) ? "vi" : "en";
        byte[] bytes = excelGenerator.Generate(data, language);

        const string windowsInvalidCharacters = "<>:\"/\\|?*";
        string stem = new string(data.Name
            .Select(c => char.IsControl(c) || windowsInvalidCharacters.Contains(c) ? '-' : c)
            .ToArray()).Trim();
        if (string.IsNullOrWhiteSpace(stem))
        {
            stem = query.Id.ToString();
        }

        return new MilestoneEventExcelFileDto
        {
            Content = bytes,
            FileName = $"{stem}-{data.Date:yyyy-MM-dd}.xlsx"
        };
    }
}
