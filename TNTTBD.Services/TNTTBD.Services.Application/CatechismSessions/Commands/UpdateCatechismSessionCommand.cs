using MediatR;

namespace TNTTBD.Services.Application.CatechismSessions.Commands;

public record UpdateCatechismSessionCommand(
    Guid CatechismSessionId,
    DateTimeOffset DateTime,
    string Title
) : IRequest<bool>;