using MediatR;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Common.Exceptions;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.CatechismYears.Commands;

public class RemoveCatechismYearHandler : IRequestHandler<RemoveCatechismYearCommand>
{
    private readonly ICatechismYearRepository _repository;
    private readonly IAuthorizationContext _authContext;

    public RemoveCatechismYearHandler(ICatechismYearRepository repository, IAuthorizationContext authContext)
    {
        _repository = repository;
        _authContext = authContext;
    }

    public async Task Handle(RemoveCatechismYearCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanWriteYearsAsync(cancellationToken))
        {
            throw new ForbiddenException();
        }

        await _repository.Remove(command.CatechismYearId, cancellationToken);
    }
}