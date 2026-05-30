using MediatR;

namespace TNTTBD.Services.Application.Auth.Commands;

public record LogoutCommand(Guid UserId) : IRequest;
