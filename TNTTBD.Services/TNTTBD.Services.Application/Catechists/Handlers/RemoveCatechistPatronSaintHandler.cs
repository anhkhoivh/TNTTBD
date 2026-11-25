using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.ValueObjects;

namespace TNTTBD.Services.Application.Catechists.Commands;

public class RemoveCatechistPatronSaintHandler : IRequestHandler<RemoveCatechistPatronSaintCommand, bool>
{
    private readonly ICatechistRepository _repository;
    private readonly IAuthorizationContext _authContext;

    public RemoveCatechistPatronSaintHandler(ICatechistRepository repository, IAuthorizationContext authContext)
    {
        _repository = repository;
        _authContext = authContext;
    }

    public async Task<bool> Handle(RemoveCatechistPatronSaintCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanAccessCatechistAsync(command.CatechistId, cancellationToken))
        {
            return false;
        }

        PatronSaint patronSaint = new PatronSaint(command.SaintName, command.Day, command.Month);
        await _repository.RemovePatronSaint(command.CatechistId, patronSaint, cancellationToken);
        return true;
    }
}