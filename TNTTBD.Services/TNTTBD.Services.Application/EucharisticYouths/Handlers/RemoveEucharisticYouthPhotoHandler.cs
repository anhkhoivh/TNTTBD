using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public class RemoveEucharisticYouthPhotoHandler : IRequestHandler<RemoveEucharisticYouthPhotoCommand, bool>
{
    private readonly IEucharisticYouthRepository _repository;
    private readonly IFileStorage _storage;
    private readonly IAuthorizationContext _authContext;

    public RemoveEucharisticYouthPhotoHandler(IEucharisticYouthRepository repository, IFileStorage storage, IAuthorizationContext authContext)
    {
        _repository = repository;
        _storage = storage;
        _authContext = authContext;
    }

    public async Task<bool> Handle(RemoveEucharisticYouthPhotoCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanAccessYouthAsync(command.EucharisticYouthId, cancellationToken))
        {
            return false;
        }

        string storageKey = await _repository.RemovePhoto(command.EucharisticYouthId, command.PhotoId, cancellationToken);
        await _storage.DeleteAsync(storageKey);
        return true;
    }
}