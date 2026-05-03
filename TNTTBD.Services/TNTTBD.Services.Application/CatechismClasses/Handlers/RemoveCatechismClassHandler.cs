using MediatR;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.CatechismClasses.Commands;

public class RemoveCatechismClassHandler : IRequestHandler<RemoveCatechismClassCommand>
{
    private readonly ICatechismClassRepository _repository;

    public RemoveCatechismClassHandler(ICatechismClassRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(RemoveCatechismClassCommand command, CancellationToken cancellationToken)
    {
        await _repository.Remove(command.CatechismClassId, cancellationToken);
    }
}