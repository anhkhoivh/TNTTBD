using MediatR;

namespace TNTTBD.Services.Application.CatechismSessions.Commands;

public record CreateCatechismSessionCommand(
    DateTimeOffset DateTime,
    string Title,
    Guid CatechismClassId
) : IRequest<Guid>;