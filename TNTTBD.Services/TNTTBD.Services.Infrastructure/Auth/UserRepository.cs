using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TNTTBD.Services.Application.Auth.DTOs;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.Users.Commands;
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

    public async Task<UserDto?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _context.Users
            .AsNoTracking()
            .Where(u => u.Email.ToLower() == email.ToLower())
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
                FullName = u.CatechistId != null ? u.Catechist.FullName
                    : u.EucharisticYouthId != null ? u.EucharisticYouth.FullName
                    : u.ContactId != null ? u.Contact.FullName
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

    public async Task<bool> ValidatePasswordAsync(string email, string password, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

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

    public async Task UpdateAsync(Guid id, string? userName, bool? isActive, UserRole? role, UserEntityLink? link, CancellationToken cancellationToken)
    {
        var entity = await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken)
            ?? throw new KeyNotFoundException($"User {id} not found.");

        if (userName != null) entity.UserName = userName;
        if (isActive.HasValue) entity.IsActive = isActive.Value;
        if (role.HasValue) entity.Role = role.Value;

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

    public async Task<PaginatedResult<UserDto>> GetAllAsync(int page, int? pageSize, CancellationToken cancellationToken)
    {
        var query = _context.Users.AsNoTracking();
        int total = await query.CountAsync(cancellationToken);

        if (pageSize.HasValue)
        {
            query = query.Skip((page - 1) * pageSize.Value).Take(pageSize.Value);
        }

        var items = await query
            .OrderBy(u => u.CreatedAt)
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
