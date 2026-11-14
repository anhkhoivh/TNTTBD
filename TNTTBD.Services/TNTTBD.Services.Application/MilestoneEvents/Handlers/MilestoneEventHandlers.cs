using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Common;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.Common.Exceptions;
using TNTTBD.Services.Application.MilestoneEvents.Commands;
using TNTTBD.Services.Application.MilestoneEvents.DTOs;
using TNTTBD.Services.Application.MilestoneEvents.Interfaces;
using TNTTBD.Services.Application.MilestoneEvents.Queries;
using TNTTBD.Services.Domain.Catechism;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Common;

namespace TNTTBD.Services.Application.MilestoneEvents.Handlers;

/// <summary>
/// Any signed-in user may view events (parents / youths get a limited view); Admins and Catechists edit; only Admins delete.
/// </summary>
internal static class MilestoneEventAccess
{
    public static async Task EnsureAdminAsync(this IAuthorizationContext auth, CancellationToken cancellationToken)
    {
        if (!await auth.IsAdminAsync(cancellationToken))
        {
            throw new ForbiddenException();
        }
    }

    public static async Task EnsureEditorAsync(this IAuthorizationContext auth, CancellationToken cancellationToken)
    {
        if (!await auth.IsAdminOrCatechistAsync(cancellationToken))
        {
            throw new ForbiddenException();
        }
    }

    public static void EnsureSignedIn(this IAuthorizationContext auth)
    {
        if (auth.GetCurrentUserId() == null)
        {
            throw new ForbiddenException();
        }
    }
}

public class SaveMilestoneEventHandler(
    IMilestoneEventRepository repository,
    IEucharisticYouthContactRepository youthContactRepository,
    IAuthorizationContext auth) : IRequestHandler<SaveMilestoneEventCommand, Guid>
{
    public const int MaxParticipants = 1000;

    public async Task<Guid> Handle(SaveMilestoneEventCommand command, CancellationToken cancellationToken)
    {
        await auth.EnsureEditorAsync(cancellationToken);

        List<SaveMilestoneEventParticipantDto> rows = command.Participants ?? [];
        if (rows.Count > MaxParticipants)
        {
            throw new DomainException(DomainErrors.ValidationRequired);
        }
        if (rows.GroupBy(p => p.EucharisticYouthId).Any(g => g.Count() > 1))
        {
            throw new DomainException(DomainErrors.MilestoneEventDuplicateYouth);
        }
        if (!string.IsNullOrWhiteSpace(command.CodeTemplate))
        {
            MilestoneCodeTemplate.Parse(command.CodeTemplate);
        }

        MilestoneEvent milestoneEvent;
        bool isNew = command.Id == null;
        if (isNew)
        {
            milestoneEvent = new MilestoneEvent(Guid.NewGuid(), command.Name, command.Type, command.Date, command.Place, command.Celebrant, command.Note, command.CodeTemplate, command.StartIndex);
        }
        else
        {
            milestoneEvent = await repository.GetAsync(command.Id!.Value, cancellationToken)
                ?? throw new DomainException(DomainErrors.ValidationNotFound);
            int count = await repository.CountParticipantsAsync(milestoneEvent.Id, cancellationToken);
            milestoneEvent.Update(command.Name, command.Type, command.Date, command.Place, command.Celebrant, command.Note, command.CodeTemplate, command.StartIndex, count > 0);
        }

        List<MilestoneEventParticipant> participants = rows
            .Select(p => new MilestoneEventParticipant(
                p.EucharisticYouthId,
                p.MilestoneId,
                p.Date,
                string.IsNullOrWhiteSpace(p.Place) ? null : p.Place.Trim(),
                p.DocumentCode,
                p.SponsorContactId,
                string.IsNullOrWhiteSpace(p.Celebrant) ? null : p.Celebrant.Trim(),
                string.IsNullOrWhiteSpace(p.Note) ? null : p.Note.Trim()))
            .ToList();

        await repository.SaveAsync(milestoneEvent, isNew, participants, auth.GetCurrentUserId(), cancellationToken);
        foreach (MilestoneEventParticipant p in participants)
        {
            await youthContactRepository.LinkGodparentAsync(p.EucharisticYouthId, milestoneEvent.Type, p.SponsorContactId, cancellationToken);
        }
        return milestoneEvent.Id;
    }
}

public class DeleteMilestoneEventHandler(
    IMilestoneEventRepository repository,
    IAuthorizationContext auth) : IRequestHandler<DeleteMilestoneEventCommand, bool>
{
    public async Task<bool> Handle(DeleteMilestoneEventCommand command, CancellationToken cancellationToken)
    {
        await auth.EnsureAdminAsync(cancellationToken);
        await repository.DeleteAsync(command.Id, cancellationToken);
        return true;
    }
}

public class GetMilestoneEventsHandler(
    IMilestoneEventQueryService queryService,
    IAuthorizationContext auth) : IRequestHandler<GetMilestoneEventsQuery, PaginatedResult<MilestoneEventSummaryDto>>
{
    public async Task<PaginatedResult<MilestoneEventSummaryDto>> Handle(GetMilestoneEventsQuery query, CancellationToken cancellationToken)
    {
        auth.EnsureSignedIn();
        return await queryService.GetListAsync(query.Search, query.Type, query.From, query.To, Math.Max(1, query.Page), query.PageSize, cancellationToken);
    }
}

public class GetMilestoneEventHandler(
    IMilestoneEventQueryService queryService,
    IAuthorizationContext auth) : IRequestHandler<GetMilestoneEventQuery, MilestoneEventDetailDto?>
{
    public async Task<MilestoneEventDetailDto?> Handle(GetMilestoneEventQuery query, CancellationToken cancellationToken)
    {
        auth.EnsureSignedIn();
        MilestoneEventDetailDto? detail = await queryService.GetDetailAsync(query.Id, cancellationToken);
        if (detail != null && !await auth.IsAdminOrCatechistAsync(cancellationToken))
        {
            // Parents / youths see who took part, not other children's personal data or godparent phone numbers.
            detail.CodeTemplate = null;
            foreach (MilestoneEventParticipantDto p in detail.Participants)
            {
                p.YouthCode = string.Empty;
                p.DateOfBirth = null;
                p.BaptismDate = null;
                p.DocumentCode = null;
                p.Sponsor = null;
                p.Note = null;
                p.Warnings = [];
            }
        }
        return detail;
    }
}

public class GetMilestoneEventCandidatesHandler(
    IMilestoneEventQueryService queryService,
    IAuthorizationContext auth) : IRequestHandler<GetMilestoneEventCandidatesQuery, PaginatedResult<MilestoneEventCandidateDto>>
{
    public async Task<PaginatedResult<MilestoneEventCandidateDto>> Handle(GetMilestoneEventCandidatesQuery query, CancellationToken cancellationToken)
    {
        await auth.EnsureEditorAsync(cancellationToken);
        int? pageSize = query.PageSize is null or < 1 ? 20 : Math.Min(query.PageSize.Value, 100);
        return await queryService.GetCandidatesAsync(query.Type, query.Date, query.Search, query.ClassId, query.CatechismYearId, Math.Max(1, query.Page), pageSize, cancellationToken);
    }
}

public class GenerateMilestoneCodesHandler(IAuthorizationContext auth)
    : IRequestHandler<GenerateMilestoneCodesQuery, GenerateMilestoneCodesResultDto>
{
    public async Task<GenerateMilestoneCodesResultDto> Handle(GenerateMilestoneCodesQuery query, CancellationToken cancellationToken)
    {
        await auth.EnsureEditorAsync(cancellationToken);
        List<DateOnly> dates = query.Dates ?? [];
        if (dates.Count > SaveMilestoneEventHandler.MaxParticipants || query.StartIndex < 0)
        {
            throw new DomainException(DomainErrors.MilestoneEventInvalidTemplate);
        }
        MilestoneCodeTemplate template = MilestoneCodeTemplate.Parse(query.Template);
        return new GenerateMilestoneCodesResultDto
        {
            Codes = dates.Select((date, i) => template.Generate(query.StartIndex + i, date)).ToList()
        };
    }
}

public class ExportMilestoneEventExcelHandler(
    IMilestoneEventQueryService queryService,
    IMilestoneEventExcelGenerator excelGenerator,
    IAuthorizationContext auth) : IRequestHandler<ExportMilestoneEventExcelQuery, MilestoneEventExcelFileDto?>
{
    public async Task<MilestoneEventExcelFileDto?> Handle(ExportMilestoneEventExcelQuery query, CancellationToken cancellationToken)
    {
        await auth.EnsureEditorAsync(cancellationToken);
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
