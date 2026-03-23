using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.Catechists.Commands;

public class RemoveCatechistHandler : IRequestHandler<RemoveCatechistCommand>
{
    private readonly ICatechistRepository _repository;

    public RemoveCatechistHandler(ICatechistRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(RemoveCatechistCommand command, CancellationToken cancellationToken)
    {
        await _repository.Remove(command.CatechistId, cancellationToken);
    }
}