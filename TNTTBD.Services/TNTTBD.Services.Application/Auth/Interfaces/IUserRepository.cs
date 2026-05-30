using TNTTBD.Services.Application.Auth.DTOs;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.Auth.Interfaces;

public interface IUserRepository
{
    Task<UserDto?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<UserDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> ValidatePasswordAsync(string email, string password, CancellationToken cancellationToken);
    Task<Guid> CreateAsync(string email, string userName, string password, UserRole role, CancellationToken cancellationToken);
    Task UpdateAsync(Guid id, string? userName, bool? isActive, UserRole? role, CancellationToken cancellationToken);
    Task<PaginatedResult<UserDto>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken);
}
