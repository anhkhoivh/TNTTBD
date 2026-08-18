using MediatR;
using TNTTBD.Services.Application.Auth.DTOs;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Application.Users.Queries;

public record GetUsersQuery(int Page = 1, int? PageSize = null) : IRequest<PaginatedResult<UserDto>>;
