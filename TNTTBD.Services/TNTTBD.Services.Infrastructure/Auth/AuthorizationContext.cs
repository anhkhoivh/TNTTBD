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

    public async Task<Guid?> GetCurrentCatechistIdAsync(CancellationToken cancellationToken)
    {
        var user = await GetCurrentUserAsync(cancellationToken);
        return user?.CatechistId;
    }

    public async Task<bool> IsAdminAsync(CancellationToken cancellationToken)
    {
        var user = await GetCurrentUserAsync(cancellationToken);
        return user?.Role == UserRole.Admin;
    }
    
    public async Task<bool> IsCatechistAsync(CancellationToken cancellationToken)
    {
        var user = await GetCurrentUserAsync(cancellationToken);
        return user?.Role == UserRole.Catechist;
    }

    public async Task<bool> IsAdminOrCatechistAsync(CancellationToken cancellationToken)
    {
        var user = await GetCurrentUserAsync(cancellationToken);
        return user?.Role == UserRole.Admin || user?.Role == UserRole.Catechist;
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

        // Any catechist can manage any youth, regardless of class assignment.
        if (user.Role is UserRole.Admin or UserRole.Catechist)
        {
            return true;
        }

        // Youth can access self
        return user.Role == UserRole.EucharisticYouth && user.EucharisticYouthId == youthId;
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

    public async Task<Guid?> GetCurrentContactIdAsync(CancellationToken cancellationToken)
    {
        var user = await GetCurrentUserAsync(cancellationToken);
        return user?.Role == UserRole.EucharisticYouthContact ? user.ContactId : null;
    }

    public async Task<bool> CanAccessContactAsync(Guid contactId, CancellationToken cancellationToken)
    {
        var user = await GetCurrentUserAsync(cancellationToken);
        if (user == null)
        {
            return false;
        }

        if (user.Role == UserRole.Admin || user.Role == UserRole.Catechist)
        {
            return true;
        }

        // Contact can access self
        if (user.Role == UserRole.EucharisticYouthContact && user.ContactId == contactId)
        {
            return true;
        }

        // Youth can access their own contacts
        if (user.Role == UserRole.EucharisticYouth && user.EucharisticYouthId.HasValue)
        {
            return await _dbContext.EucharisticYouthContacts
                .AnyAsync(yc => yc.ContactId == contactId
                    && yc.EucharisticYouthId == user.EucharisticYouthId.Value,
                    cancellationToken);
        }

        return false;
    }

    public async Task<bool> CanReadYouthContactsAsync(Guid youthId, CancellationToken cancellationToken)
    {
        if (await CanAccessYouthAsync(youthId, cancellationToken))
        {
            return true;
        }

        // Contact linked to the youth
        var contactId = await GetCurrentContactIdAsync(cancellationToken);
        if (contactId.HasValue)
        {
            return await _dbContext.EucharisticYouthContacts
                .AnyAsync(yc => yc.ContactId == contactId.Value
                    && yc.EucharisticYouthId == youthId,
                    cancellationToken);
        }

        return false;
    }

    public Task<bool> CanViewYouthAsync(Guid youthId, CancellationToken cancellationToken)
    {
        return CanManageYouthPhotosAsync(youthId, cancellationToken);
    }

    public async Task<bool> CanViewCatechistAsync(Guid catechistId, CancellationToken cancellationToken)
    {
        var user = await GetCurrentUserAsync(cancellationToken);
        return user?.Role is UserRole.Admin or UserRole.Catechist;
    }

    public async Task<bool> CanManageCatechistPhotosAsync(Guid catechistId, CancellationToken cancellationToken)
    {
        var user = await GetCurrentUserAsync(cancellationToken);
        return user != null && (user.Role == UserRole.Admin
            || (user.Role == UserRole.Catechist && user.CatechistId == catechistId));
    }

    public async Task<bool> CanManageYouthPhotosAsync(Guid youthId, CancellationToken cancellationToken)
    {
        var user = await GetCurrentUserAsync(cancellationToken);
        if (user == null)
        {
            return false;
        }

        if (user.Role is UserRole.Admin or UserRole.Catechist)
        {
            return true;
        }

        if (user.Role == UserRole.EucharisticYouth)
        {
            return user.EucharisticYouthId == youthId;
        }

        if (user.Role == UserRole.EucharisticYouthContact && user.ContactId.HasValue)
        {
            return await _dbContext.EucharisticYouthContacts
                .AnyAsync(yc => yc.ContactId == user.ContactId.Value
                    && yc.EucharisticYouthId == youthId,
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
