using MediatR;
using TNTTBD.Services.Application.CatechismClassCatechists.Commands;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.CatechismClassCatechists.Handlers;

public class RemoveCatechistFromClassHandler : IRequestHandler<RemoveCatechistFromClassCommand>
{
    private readonly ICatechismClassCatechistRepository _repository;

    public RemoveCatechistFromClassHandler(ICatechismClassCatechistRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(RemoveCatechistFromClassCommand command, CancellationToken cancellationToken)
    {
        await _repository.Remove(command.Id, cancellationToken);
    }
}
