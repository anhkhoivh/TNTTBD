using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.Contacts.DTOs;

namespace TNTTBD.Services.Application.Contacts.Interfaces;

public interface IContactQueryService
{
    Task<PaginatedResult<ContactDto>> GetAllAsync(int page, int? pageSize, string? search, CancellationToken cancellationToken);
    Task<ContactDetailDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
