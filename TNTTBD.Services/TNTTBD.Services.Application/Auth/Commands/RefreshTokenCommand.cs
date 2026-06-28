using MediatR;
using TNTTBD.Services.Application.Auth.DTOs;

namespace TNTTBD.Services.Application.Auth.Commands;

public record RefreshTokenCommand(string RefreshToken, Guid UserId) : IRequest<LoginResponse>;
