using MediatR;
using TNTTBD.Services.Application.Auth.DTOs;

namespace TNTTBD.Services.Application.Auth.Commands;

public record LoginCommand(string Email, string Password) : IRequest<LoginResponse>;
