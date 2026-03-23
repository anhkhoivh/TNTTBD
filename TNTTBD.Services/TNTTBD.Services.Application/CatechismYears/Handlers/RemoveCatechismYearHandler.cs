using MediatR;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.CatechismYears.Commands;

public class RemoveCatechismYearHandler : IRequestHandler<RemoveCatechismYearCommand>
{
    private readonly ICatechismYearRepository _repository;

    public RemoveCatechismYearHandler(ICatechismYearRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(RemoveCatechismYearCommand command, CancellationToken cancellationToken)
    {
        await _repository.Remove(command.CatechismYearId, cancellationToken);
    }
}