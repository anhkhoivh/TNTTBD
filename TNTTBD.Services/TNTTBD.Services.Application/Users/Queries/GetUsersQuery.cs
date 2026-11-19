using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.Users.Queries;

public record GetUsersQuery(int Page = 1, int? PageSize = null, string? Search = null, UserRole? Role = null);
