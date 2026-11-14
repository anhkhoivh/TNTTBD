using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.MilestoneEvents.DTOs;
using TNTTBD.Services.Application.MilestoneEvents.Interfaces;
using TNTTBD.Services.Domain.Catechism;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.QueryServices;

public class MilestoneEventQueryService : IMilestoneEventQueryService
{
    private readonly CatechismDbContext _context;

    public MilestoneEventQueryService(CatechismDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<MilestoneEventSummaryDto>> GetListAsync(string? search, MilestoneType? type, DateOnly? from, DateOnly? to, int page, int? pageSize, CancellationToken cancellationToken)
    {
        var query = _context.MilestoneEvents.AsNoTracking();
        if (type.HasValue)
        {
            query = query.Where(e => e.Type == type.Value);
        }
        if (from.HasValue)
        {
            query = query.Where(e => e.Date >= from.Value);
        }
        if (to.HasValue)
        {
            query = query.Where(e => e.Date <= to.Value);
        }
        if (!string.IsNullOrWhiteSpace(search))
        {
            string term = search.Trim().ToLower();
            query = query.Where(e => e.Name.ToLower().Contains(term) || (e.Place != null && e.Place.ToLower().Contains(term)));
        }

        int total = await query.CountAsync(cancellationToken);
        query = query.OrderByDescending(e => e.Date).ThenBy(e => e.Name);
        if (pageSize.HasValue)
        {
            query = query.Skip((page - 1) * pageSize.Value).Take(pageSize.Value);
        }

        List<MilestoneEventSummaryDto> items = await query
            .Select(e => new MilestoneEventSummaryDto
            {
                Id = e.Id,
                Name = e.Name,
                Type = e.Type,
                Date = e.Date,
                Place = e.Place,
                ParticipantCount = e.Milestones.Count
            })
            .ToListAsync(cancellationToken);

        return new PaginatedResult<MilestoneEventSummaryDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize ?? total
        };
    }

    /// <inheritdoc />
    public async Task<MilestoneEventDetailDto?> GetDetailAsync(Guid id, CancellationToken cancellationToken)
    {
        MilestoneEventDetailDto? detail = await _context.MilestoneEvents
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new MilestoneEventDetailDto
            {
                Id = e.Id,
                Name = e.Name,
                Type = e.Type,
                Date = e.Date,
                Place = e.Place,
                Celebrant = e.Celebrant,
                Note = e.Note,
                CodeTemplate = e.CodeTemplate,
                StartIndex = e.StartIndex
            })
            .FirstOrDefaultAsync(cancellationToken);
        if (detail == null)
        {
            return null;
        }

        detail.Participants = await _context.SpiritualMilestones
            .AsNoTracking()
            .Where(m => m.MilestoneEventId == id)
            .OrderBy(m => m.EucharisticYouth.FullName.GivenName)
            .ThenBy(m => m.EucharisticYouth.FullName.FamilyName)
            .ThenBy(m => m.EucharisticYouth.FullName.MiddleName)
            .Select(m => new MilestoneEventParticipantDto
            {
                MilestoneId = m.Id,
                EucharisticYouthId = m.EucharisticYouthId,
                YouthCode = m.EucharisticYouth.Code,
                FullName = m.EucharisticYouth.FullName,
                DateOfBirth = m.EucharisticYouth.DateOfBirth,
                Date = m.Date,
                Place = m.Place,
                Celebrant = m.Celebrant,
                DocumentCode = m.DocumentCode,
                Sponsor = m.SponsorContact == null ? null : new MilestoneSponsorDto
                {
                    ContactId = m.SponsorContact.Id,
                    FullName = m.SponsorContact.FullName,
                    PhoneNumber = m.SponsorContact.PhoneNumber
                },
                Note = m.Note,
                BaptismDate = m.EucharisticYouth.SpiritualMilestones
                    .Where(b => b.Type == MilestoneType.Baptism)
                    .Select(b => b.Date)
                    .FirstOrDefault()
            })
            .ToListAsync(cancellationToken);

        List<Guid> youthIds = detail.Participants.Select(p => p.EucharisticYouthId).ToList();
        Dictionary<Guid, string> classNames = await GetCurrentClassNamesAsync(youthIds, cancellationToken);
        ILookup<Guid, (MilestoneType Type, DateOnly? Date)> recorded = await GetRecordedMilestonesAsync(youthIds, cancellationToken);
        foreach (MilestoneEventParticipantDto p in detail.Participants)
        {
            p.ClassName = classNames.GetValueOrDefault(p.EucharisticYouthId);
            p.Warnings = ToWarnings(detail.Type, p.Date ?? detail.Date, recorded[p.EucharisticYouthId]);
        }
        return detail;
    }

    /// <inheritdoc />
    public async Task<PaginatedResult<MilestoneEventCandidateDto>> GetCandidatesAsync(MilestoneType type, DateOnly? date, string? search, Guid? classId, Guid? catechismYearId, int page, int? pageSize, CancellationToken cancellationToken)
    {
        var query = _context.EucharisticYouths.AsNoTracking();

        if (classId.HasValue)
        {
            query = query.Where(y => _context.CatechismClassEucharisticYouths.Any(cy =>
                cy.EucharisticYouthId == y.Id && cy.CatechismClassId == classId.Value && cy.Status == EnrollmentStatus.Active));
        }
        if (catechismYearId.HasValue)
        {
            query = query.Where(y => _context.CatechismClassEucharisticYouths.Any(cy =>
                cy.EucharisticYouthId == y.Id && cy.CatechismClass.CatechismYearId == catechismYearId.Value && cy.Status == EnrollmentStatus.Active));
        }
        if (!string.IsNullOrWhiteSpace(search))
        {
            string term = search.Trim().ToLower();
            query = query.Where(y =>
                y.Code.ToLower().Contains(term) ||
                (y.FullName.GivenName != null && y.FullName.GivenName.ToLower().Contains(term)) ||
                (y.FullName.MiddleName != null && y.FullName.MiddleName.ToLower().Contains(term)) ||
                (y.FullName.FamilyName != null && y.FullName.FamilyName.ToLower().Contains(term)) ||
                (y.FullName.ChristName != null && y.FullName.ChristName.ToLower().Contains(term)));
        }

        int total = await query.CountAsync(cancellationToken);
        query = query.OrderBy(y => y.FullName.GivenName).ThenBy(y => y.FullName.FamilyName).ThenBy(y => y.FullName.MiddleName);
        if (pageSize.HasValue)
        {
            query = query.Skip((page - 1) * pageSize.Value).Take(pageSize.Value);
        }

        List<MilestoneEventCandidateDto> items = await query
            .Select(y => new MilestoneEventCandidateDto
            {
                EucharisticYouthId = y.Id,
                YouthCode = y.Code,
                FullName = y.FullName,
                DateOfBirth = y.DateOfBirth,
                HasBaptism = y.SpiritualMilestones.Any(m => m.Type == MilestoneType.Baptism),
                BaptismDate = y.SpiritualMilestones
                    .Where(m => m.Type == MilestoneType.Baptism)
                    .Select(m => m.Date)
                    .FirstOrDefault(),
                ExistingMilestone = y.SpiritualMilestones
                    .Where(m => m.Type == type)
                    .Select(m => new MilestoneEventCandidateMilestoneDto
                    {
                        MilestoneId = m.Id,
                        Date = m.Date,
                        Place = m.Place,
                        Celebrant = m.Celebrant,
                        DocumentCode = m.DocumentCode,
                        Sponsor = m.SponsorContact == null ? null : new MilestoneSponsorDto
                        {
                            ContactId = m.SponsorContact.Id,
                            FullName = m.SponsorContact.FullName,
                            PhoneNumber = m.SponsorContact.PhoneNumber
                        },
                        Note = m.Note,
                        MilestoneEventId = m.MilestoneEventId,
                        MilestoneEventName = m.MilestoneEvent != null ? m.MilestoneEvent.Name : null
                    })
                    .FirstOrDefault()
            })
            .ToListAsync(cancellationToken);

        List<Guid> candidateIds = items.Select(i => i.EucharisticYouthId).ToList();
        Dictionary<Guid, string> classNames = await GetCurrentClassNamesAsync(candidateIds, cancellationToken);
        ILookup<Guid, (MilestoneType Type, DateOnly? Date)> recorded = await GetRecordedMilestonesAsync(candidateIds, cancellationToken);
        foreach (MilestoneEventCandidateDto item in items)
        {
            item.ClassName = classNames.GetValueOrDefault(item.EucharisticYouthId);
            item.Warnings = ToWarnings(type, date ?? item.ExistingMilestone?.Date, recorded[item.EucharisticYouthId]);
        }

        return new PaginatedResult<MilestoneEventCandidateDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize ?? total
        };
    }

    /// <inheritdoc />
    public async Task<MilestoneEventExportDto?> GetExportDataAsync(Guid id, CancellationToken cancellationToken)
    {
        MilestoneEventExportDto? data = await _context.MilestoneEvents
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new MilestoneEventExportDto { Name = e.Name, Type = e.Type, Date = e.Date })
            .FirstOrDefaultAsync(cancellationToken);
        if (data == null)
        {
            return null;
        }

        var rows = await _context.SpiritualMilestones
            .AsNoTracking()
            .Where(m => m.MilestoneEventId == id)
            .OrderBy(m => m.EucharisticYouth.FullName.GivenName)
            .ThenBy(m => m.EucharisticYouth.FullName.FamilyName)
            .ThenBy(m => m.EucharisticYouth.FullName.MiddleName)
            .Select(m => new
            {
                m.EucharisticYouthId,
                Row = new MilestoneEventExportRowDto
                {
                    DocumentCode = m.DocumentCode,
                    FullName = m.EucharisticYouth.FullName,
                    DateOfBirth = m.EucharisticYouth.DateOfBirth,
                    Address = m.EucharisticYouth.Address,
                    SponsorFullName = m.SponsorContact != null ? m.SponsorContact.FullName : null,
                    Celebrant = m.Celebrant ?? m.MilestoneEvent!.Celebrant,
                    BaptismDate = m.EucharisticYouth.SpiritualMilestones
                        .Where(b => b.Type == MilestoneType.Baptism)
                        .Select(b => b.Date)
                        .FirstOrDefault()
                }
            })
            .ToListAsync(cancellationToken);

        List<Guid> youthIds = rows.Select(r => r.EucharisticYouthId).ToList();
        var parents = await _context.EucharisticYouthContacts
            .AsNoTracking()
            .Where(c => youthIds.Contains(c.EucharisticYouthId)
                && (c.ContactRelationType == ContactRelationType.Father || c.ContactRelationType == ContactRelationType.Mother))
            .Select(c => new { c.EucharisticYouthId, c.ContactId, c.ContactRelationType, c.Contact.FullName })
            .ToListAsync(cancellationToken);

        foreach (var r in rows)
        {
            r.Row.Parents = parents
                .Where(p => p.EucharisticYouthId == r.EucharisticYouthId)
                .GroupBy(p => new { p.ContactId, p.ContactRelationType })
                .Select(g => g.First())
                .OrderBy(p => p.ContactRelationType)
                .ThenBy(p => p.FullName.ToString())
                .Select(p => new MilestoneEventExportParentDto { Relation = p.ContactRelationType, FullName = p.FullName })
                .ToList();
            data.Rows.Add(r.Row);
        }
        return data;
    }

    private async Task<ILookup<Guid, (MilestoneType Type, DateOnly? Date)>> GetRecordedMilestonesAsync(List<Guid> youthIds, CancellationToken cancellationToken)
    {
        if (youthIds.Count == 0)
        {
            return Array.Empty<(Guid, MilestoneType, DateOnly?)>().ToLookup(x => x.Item1, x => (x.Item2, x.Item3));
        }
        var rows = await _context.SpiritualMilestones
            .AsNoTracking()
            .Where(m => youthIds.Contains(m.EucharisticYouthId))
            .Select(m => new { m.EucharisticYouthId, m.Type, m.Date })
            .ToListAsync(cancellationToken);
        return rows.ToLookup(r => r.EucharisticYouthId, r => (r.Type, r.Date));
    }

    private static List<MilestonePrerequisiteWarningDto> ToWarnings(MilestoneType type, DateOnly? date, IEnumerable<(MilestoneType Type, DateOnly? Date)> recorded) =>
        MilestonePrerequisites.Check(type, date, recorded)
            .Select(w => new MilestonePrerequisiteWarningDto { Type = w.Type, Issue = w.Issue })
            .ToList();

    /// <summary>
    /// Active class for each youth, preferring the most recent catechism year.
    /// </summary>
    private async Task<Dictionary<Guid, string>> GetCurrentClassNamesAsync(List<Guid> youthIds, CancellationToken cancellationToken)
    {
        if (youthIds.Count == 0)
        {
            return new Dictionary<Guid, string>();
        }
        var enrollments = await _context.CatechismClassEucharisticYouths
            .AsNoTracking()
            .Where(e => youthIds.Contains(e.EucharisticYouthId) && e.Status == EnrollmentStatus.Active)
            .Select(e => new { e.EucharisticYouthId, e.CatechismClass.ClassName, e.CatechismClass.CatechismYear.StartDate })
            .ToListAsync(cancellationToken);

        return enrollments
            .GroupBy(e => e.EucharisticYouthId)
            .ToDictionary(g => g.Key, g => g.OrderByDescending(e => e.StartDate).First().ClassName);
    }
}
