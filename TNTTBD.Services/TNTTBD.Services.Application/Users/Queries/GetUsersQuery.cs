using MediatR;
using TNTTBD.Services.Application.Auth.DTOs;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.Users.Queries;

public record GetUsersQuery(int Page = 1, int? PageSize = null, string? Search = null, UserRole? Role = null) : IRequest<PaginatedResult<UserDto>>;
