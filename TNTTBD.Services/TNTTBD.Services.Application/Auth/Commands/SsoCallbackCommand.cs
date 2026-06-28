using MediatR;
using TNTTBD.Services.Application.Auth.DTOs;

namespace TNTTBD.Services.Application.Auth.Commands;

public record SsoCallbackCommand(string Provider, string Code, string RedirectUri) : IRequest<LoginResponse>;
