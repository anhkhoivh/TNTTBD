using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.Catechists.Commands;

public class RemoveCatechistPatronSaintHandler : IRequestHandler<RemoveCatechistPatronSaintCommand>
{
    private readonly ICatechistRepository _repository;

    public RemoveCatechistPatronSaintHandler(ICatechistRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(RemoveCatechistPatronSaintCommand command, CancellationToken cancellationToken)
    {
        PatronSaint patronSaint = new PatronSaint(command.SaintName, command.Day, command.Month);
        await _repository.RemovePatronSaint(command.CatechistId, patronSaint, cancellationToken);
    }
}