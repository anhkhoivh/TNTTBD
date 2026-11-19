using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.Catechists.Commands;

public class RemoveCatechistHandler : IRequestHandler<RemoveCatechistCommand, bool>
{
    private readonly ICatechistRepository _repository;
    private readonly IAuthorizationContext _authContext;

    public RemoveCatechistHandler(ICatechistRepository repository, IAuthorizationContext authContext)
    {
        _repository = repository;
        _authContext = authContext;
    }

    public async Task<bool> Handle(RemoveCatechistCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.IsAdminAsync(cancellationToken))
        {
            return false;
        }

        // Linked User (if any) is hard-deleted by the DB via cascade FK (UserEntityConfiguration.CatechistId).
        await _repository.Remove(command.CatechistId, cancellationToken);
        return true;
    }
}