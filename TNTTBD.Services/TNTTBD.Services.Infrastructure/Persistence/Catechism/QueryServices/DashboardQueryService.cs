using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Application.Dashboard.DTOs;
using TNTTBD.Services.Application.Dashboard.Interfaces;
using TNTTBD.Services.Domain.Enums;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Persistence.Catechism.QueryServices;

public class DashboardQueryService : IDashboardQueryService
{
    private readonly CatechismDbContext _context;

    public DashboardQueryService(CatechismDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardSummaryDto> GetSummaryAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .Select(u => new { u.Role, u.CatechistId, u.EucharisticYouthId, u.ContactId })
            .FirstOrDefaultAsync(cancellationToken);

        var result = new DashboardSummaryDto();
        if (user == null) return result;

        CatechismYearEntity? latestYear = await _context.CatechismYears
            .AsNoTracking()
            .OrderByDescending(y => y.StartDate)
            .FirstOrDefaultAsync(cancellationToken);

        if (latestYear == null) return result;

        result.CurrentYearTitle = $"{latestYear.StartDate.Year}-{latestYear.EndDate.Year}";

        // Classes relevant to the caller, resolved by role
        var myClassIds = new List<Guid>();

        if (user.Role == UserRole.Catechist && user.CatechistId.HasValue)
        {
            myClassIds = await _context.CatechismClassCatechists
                .AsNoTracking()
                .Where(cc => cc.CatechistId == user.CatechistId.Value
                    && cc.Status == EnrollmentStatus.Active
                    && cc.CatechismClass.CatechismYearId == latestYear.Id)
                .Select(cc => cc.CatechismClassId)
                .ToListAsync(cancellationToken);
        }
        else if (user.EucharisticYouthId.HasValue)
        {
            myClassIds = await _context.CatechismClassEucharisticYouths
                .AsNoTracking()
                .Where(cy => cy.EucharisticYouthId == user.EucharisticYouthId.Value
                    && cy.Status == EnrollmentStatus.Active
                    && cy.CatechismClass.CatechismYearId == latestYear.Id)
                .Select(cy => cy.CatechismClassId)
                .ToListAsync(cancellationToken);
        }

        if (myClassIds.Count > 0)
        {
            result.MyClasses = await _context.CatechismClasses
                .AsNoTracking()
                .Where(c => myClassIds.Contains(c.Id))
                .Select(c => new DashboardClassDto { Id = c.Id, ClassName = c.ClassName })
                .ToListAsync(cancellationToken);
        }

        // Admins see all upcoming sessions; everyone else only sees their own classes'
        var now = DateTimeOffset.UtcNow;
        var sessionQuery = _context.CatechismSessions
            .AsNoTracking()
            .Where(s => s.DateTime >= now && s.CatechismClass.CatechismYearId == latestYear.Id);

        if (user.Role != UserRole.Admin)
        {
            sessionQuery = sessionQuery.Where(s => myClassIds.Contains(s.CatechismClassId));
        }

        result.UpcomingSessions = await sessionQuery
            .OrderBy(s => s.DateTime)
            .Take(5)
            .Select(s => new DashboardSessionDto
            {
                Id = s.Id,
                Title = s.Title,
                DateTime = s.DateTime,
                CatechismClassId = s.CatechismClassId,
                ClassName = s.CatechismClass.ClassName,
            })
            .ToListAsync(cancellationToken);

        if (user.Role == UserRole.Admin)
        {
            int youthCount = await _context.CatechismClassEucharisticYouths
                .AsNoTracking()
                .Where(cy => (cy.Status == EnrollmentStatus.Active || cy.Status == EnrollmentStatus.Pending || cy.Status == EnrollmentStatus.Completed)
                    && cy.CatechismClass.CatechismYearId == latestYear.Id)
                .Select(cy => cy.EucharisticYouthId)
                .Distinct()
                .CountAsync(cancellationToken);
            result.Totals = new DashboardTotalsDto
            {
                Youth = youthCount,
                Catechists = await _context.Catechists.CountAsync(cancellationToken),
                Classes = await _context.CatechismClasses.Where(c => c.CatechismYearId == latestYear.Id).CountAsync(cancellationToken),
                Years = await _context.CatechismYears.CountAsync(cancellationToken),
            };
        }

        return result;
    }
}
