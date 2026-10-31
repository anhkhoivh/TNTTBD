using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Application.Auth.DTOs;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.Users.Commands;
using TNTTBD.Services.Domain.Common;
using TNTTBD.Services.Domain.Enums;
using TNTTBD.Services.Infrastructure.Persistence.Catechism;
using TNTTBD.Services.Infrastructure.Persistence.Catechism.Entities;

namespace TNTTBD.Services.Infrastructure.Auth;

public class UserRepository : IUserRepository
{
    private readonly CatechismDbContext _context;
    private readonly IPasswordHasher<UserEntity> _passwordHasher;

    public UserRepository(CatechismDbContext context, IPasswordHasher<UserEntity> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserDto?> GetByIdentifierAsync(string identifier, CancellationToken cancellationToken)
    {
        return await _context.Users
            .AsNoTracking()
            .Where(u =>
                u.Email.ToLower() == identifier.ToLower() ||
                u.UserName == identifier)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                UserName = u.UserName,
                IsActive = u.IsActive,
                UserRole = u.Role,
                CatechistId = u.CatechistId,
                EucharisticYouthId = u.EucharisticYouthId,
                ContactId = u.ContactId,
                FullName = u.CatechistId != null ? u.Catechist!.FullName
                    : u.EucharisticYouthId != null ? u.EucharisticYouth!.FullName
                    : u.ContactId != null ? u.Contact!.FullName
                    : null

            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<UserDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Users
            .AsNoTracking()
            .Where(u => u.Id == id)
            .Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                UserName = u.UserName,
                IsActive = u.IsActive,
                UserRole = u.Role,
                CatechistId = u.CatechistId,
                EucharisticYouthId = u.EucharisticYouthId,
                ContactId = u.ContactId
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> ValidatePasswordAsync(string identifier, string password, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u =>
                u.Email.ToLower() == identifier.ToLower() ||
                u.UserName == identifier,
                cancellationToken);

        if (user?.PasswordHash == null)
            return false;

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        return result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded;
    }

    public async Task<bool> HasPasswordAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _context.Users
            .AsNoTracking()
            .AnyAsync(u => u.Id == userId && u.PasswordHash != null, cancellationToken);
    }

    public async Task<Guid> CreateAsync(string email, string userName, string password, UserRole role, Guid? catechistId, Guid? eucharisticYouthId, Guid? contactId, CancellationToken cancellationToken)
    {
        var entity = new UserEntity
        {
            Id = Guid.NewGuid(),
            Email = email,
            UserName = userName,
            Role = role,
            IsActive = true,
            CatechistId = catechistId,
            EucharisticYouthId = eucharisticYouthId,
            ContactId = contactId,
            CreatedAt = DateTimeOffset.UtcNow
        };

        entity.PasswordHash = _passwordHasher.HashPassword(entity, password);

        await _context.Users.AddAsync(entity, cancellationToken);
        return entity.Id;
    }

    public async Task UpdateAsync(Guid id, string? userName, string? email, bool? isActive, UserRole? role, UserEntityLink? link, CancellationToken cancellationToken)
    {
        var entity = await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"User {id} not found.");

        if (userName != null) entity.UserName = userName;
        if (isActive.HasValue) entity.IsActive = isActive.Value;
        if (role.HasValue) entity.Role = role.Value;

        if (email != null && !string.Equals(email, entity.Email, StringComparison.OrdinalIgnoreCase))
        {
            bool emailTaken = await _context.Users
                .AnyAsync(u => u.Id != id && u.Email.ToLower() == email.ToLower(), cancellationToken);
            if (emailTaken)
            {
                throw new DomainException(DomainErrors.EmailAlreadyExists);
            }
            entity.Email = email;
        }

        if (link != null)
        {
            entity.CatechistId = link.Kind == UserEntityLinkKind.Catechist ? link.Id : null;
            entity.EucharisticYouthId = link.Kind == UserEntityLinkKind.Youth ? link.Id : null;
            entity.ContactId = link.Kind == UserEntityLinkKind.Contact ? link.Id : null;
        }
    }

    public async Task DeactivateAsync(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"User {id} not found.");

        entity.IsActive = false;
    }

    public async Task ChangePasswordAsync(Guid userId, string newPassword, CancellationToken cancellationToken)
    {
        var entity = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
            ?? throw new KeyNotFoundException($"User {userId} not found.");

        entity.PasswordHash = _passwordHasher.HashPassword(entity, newPassword);
    }

    public async Task<PaginatedResult<UserDto>> GetAllAsync(int page, int? pageSize, string? search, UserRole? role, CancellationToken cancellationToken)
    {
        var query = _context.Users.AsNoTracking();

        if (role.HasValue)
        {
            query = query.Where(u => u.Role == role.Value);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            query = query.Where(u =>
                u.UserName.ToLower().Contains(s) ||
                u.Email.ToLower().Contains(s) ||
                (u.Catechist != null && (
                    u.Catechist.FullName.GivenName.ToLower().Contains(s) ||
                    (u.Catechist.FullName.FamilyName != null && u.Catechist.FullName.FamilyName.ToLower().Contains(s)) ||
                    (u.Catechist.FullName.MiddleName != null && u.Catechist.FullName.MiddleName.ToLower().Contains(s)) ||
                    (u.Catechist.FullName.ChristName != null && u.Catechist.FullName.ChristName.ToLower().Contains(s)))) ||
                (u.EucharisticYouth != null && (
                    u.EucharisticYouth.FullName.GivenName.ToLower().Contains(s) ||
                    (u.EucharisticYouth.FullName.FamilyName != null && u.EucharisticYouth.FullName.FamilyName.ToLower().Contains(s)) ||
                    (u.EucharisticYouth.FullName.MiddleName != null && u.EucharisticYouth.FullName.MiddleName.ToLower().Contains(s)) ||
                    (u.EucharisticYouth.FullName.ChristName != null && u.EucharisticYouth.FullName.ChristName.ToLower().Contains(s)))) ||
                (u.Contact != null && (
                    u.Contact.FullName.GivenName.ToLower().Contains(s) ||
                    (u.Contact.FullName.FamilyName != null && u.Contact.FullName.FamilyName.ToLower().Contains(s)) ||
                    (u.Contact.FullName.MiddleName != null && u.Contact.FullName.MiddleName.ToLower().Contains(s)) ||
                    (u.Contact.FullName.ChristName != null && u.Contact.FullName.ChristName.ToLower().Contains(s)))));
        }

        int total = await query.CountAsync(cancellationToken);

        query = query
            .OrderBy(u => u.CreatedAt)
            .ThenBy(u => u.Id);

        if (pageSize.HasValue)
        {
            query = query.Skip((page - 1) * pageSize.Value).Take(pageSize.Value);
        }

        var items = await query
            .Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                UserName = u.UserName,
                IsActive = u.IsActive,
                UserRole = u.Role,
                CatechistId = u.CatechistId,
                EucharisticYouthId = u.EucharisticYouthId,
                ContactId = u.ContactId,
                FullName = u.CatechistId != null ? u.Catechist!.FullName
                    : u.EucharisticYouthId != null ? u.EucharisticYouth!.FullName
                    : u.ContactId != null ? u.Contact!.FullName
                    : null
            })
            .ToListAsync(cancellationToken);

        return new PaginatedResult<UserDto>
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize ?? total
        };
    }
}
