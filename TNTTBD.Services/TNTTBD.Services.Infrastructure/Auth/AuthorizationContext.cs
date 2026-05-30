using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Domain.Enums;
using TNTTBD.Services.Infrastructure.Persistence.Catechism;

namespace TNTTBD.Services.Infrastructure.Auth;

public class AuthorizationContext : IAuthorizationContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly CatechismDbContext _dbContext;

    public AuthorizationContext(IHttpContextAccessor httpContextAccessor, CatechismDbContext dbContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
    }

    public Guid? GetCurrentUserId()
    {
        var claim = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (Guid.TryParse(claim, out var id))
        {
            return id;
        }

        return null;
    }

    public async Task<bool> IsAdminAsync(CancellationToken cancellationToken)
    {
        var user = await GetCurrentUserAsync(cancellationToken);
        return user?.Role == UserRole.Admin;
    }

    public async Task<bool> CanAccessClassAsync(Guid classId, CancellationToken cancellationToken)
    {
        var user = await GetCurrentUserAsync(cancellationToken);
        if (user == null)
        {
            return false;
        }

        if (user.Role == UserRole.Admin)
        {
            return true;
        }

        if (user.Role == UserRole.Catechist && user.CatechistId.HasValue)
        {
            return await _dbContext.CatechismClassCatechists
                .AnyAsync(cc => cc.CatechismClassId == classId
                    && cc.CatechistId == user.CatechistId.Value
                    && cc.Status == EnrollmentStatus.Active,
                    cancellationToken);
        }

        if ((user.Role == UserRole.EucharisticYouth) && user.EucharisticYouthId.HasValue)
        {
            return await _dbContext.CatechismClassEucharisticYouths
                .AnyAsync(cy => cy.CatechismClassId == classId
                    && cy.EucharisticYouthId == user.EucharisticYouthId.Value
                    && cy.Status == EnrollmentStatus.Active,
                    cancellationToken);
        }

        return false;
    }

    public async Task<bool> CanAccessYouthAsync(Guid youthId, CancellationToken cancellationToken)
    {
        var user = await GetCurrentUserAsync(cancellationToken);
        if (user == null)
        {
            return false;
        }

        if (user.Role == UserRole.Admin)
        {
            return true;
        }

        // Youth can access self
        if (user.Role == UserRole.EucharisticYouth && user.EucharisticYouthId == youthId)
        {
            return true;
        }

        // Catechist can access youths in their classes
        if (user.Role == UserRole.Catechist && user.CatechistId.HasValue)
        {
            var catechistClassIds = _dbContext.CatechismClassCatechists
                .Where(cc => cc.CatechistId == user.CatechistId.Value && cc.Status == EnrollmentStatus.Active)
                .Select(cc => cc.CatechismClassId);

            return await _dbContext.CatechismClassEucharisticYouths
                .AnyAsync(cy => cy.EucharisticYouthId == youthId
                    && catechistClassIds.Contains(cy.CatechismClassId)
                    && cy.Status == EnrollmentStatus.Active,
                    cancellationToken);
        }

        return false;
    }

    public async Task<bool> CanAccessCatechistAsync(Guid catechistId, CancellationToken cancellationToken)
    {
        var user = await GetCurrentUserAsync(cancellationToken);
        if (user == null)
        {
            return false;
        }

        if (user.Role == UserRole.Admin)
        {
            return true;
        }

        // Catechist can access self
        if (user.Role == UserRole.Catechist && user.CatechistId == catechistId)
        {
            return true;
        }

        // Other catechists/youths can read catechists in shared classes
        if (user.Role == UserRole.Catechist && user.CatechistId.HasValue)
        {
            var myClassIds = _dbContext.CatechismClassCatechists
                .Where(cc => cc.CatechistId == user.CatechistId.Value && cc.Status == EnrollmentStatus.Active)
                .Select(cc => cc.CatechismClassId);

            return await _dbContext.CatechismClassCatechists
                .AnyAsync(cc => cc.CatechistId == catechistId
                    && myClassIds.Contains(cc.CatechismClassId)
                    && cc.Status == EnrollmentStatus.Active,
                    cancellationToken);
        }

        return false;
    }

    public async Task<bool> CanWriteYearsAsync(CancellationToken cancellationToken)
    {
        var user = await GetCurrentUserAsync(cancellationToken);
        return user?.Role == UserRole.Admin;
    }

    public async Task<bool> CanWriteClassCatechistAsync(CancellationToken cancellationToken)
    {
        var user = await GetCurrentUserAsync(cancellationToken);
        return user?.Role == UserRole.Admin;
    }

    private async Task<Persistence.Catechism.Entities.UserEntity?> GetCurrentUserAsync(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            return null;
        }

        return await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId.Value && u.IsActive, cancellationToken);
    }
}
