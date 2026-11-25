using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.CatechismSessions.Commands;

public class RemoveCatechismSessionHandler : IRequestHandler<RemoveCatechismSessionCommand>
{
    private readonly ICatechismSessionRepository _repository;

    public RemoveCatechismSessionHandler(ICatechismSessionRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(RemoveCatechismSessionCommand command, CancellationToken cancellationToken)
    {
        await _repository.Remove(command.CatechismSessionId, cancellationToken);
    }
}