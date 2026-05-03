using MediatR;

namespace TNTTBD.Services.Application.CatechismSessions.Commands;

public record RemoveCatechismSessionCommand(
    Guid CatechismSessionId
) : IRequest;