using MediatR;
using TNTTBD.Services.Application.Auth.DTOs;

namespace TNTTBD.Services.Application.Users.Queries;

public record GetUserByIdQuery(Guid Id) : IRequest<UserDto?>;
