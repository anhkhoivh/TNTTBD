using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.Catechists.Commands;

public class RemoveCatechistPhotoHandler : IRequestHandler<RemoveCatechistPhotoCommand, bool>
{
    private readonly ICatechistRepository _repository;
    private readonly IFileStorage _storage;
    private readonly IAuthorizationContext _authContext;

    public RemoveCatechistPhotoHandler(ICatechistRepository repository, IFileStorage storage, IAuthorizationContext authContext)
    {
        _repository = repository;
        _storage = storage;
        _authContext = authContext;
    }

    public async Task<bool> Handle(RemoveCatechistPhotoCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanAccessCatechistAsync(command.CatechistId, cancellationToken))
        {
            return false;
        }

        string storageKey = await _repository.RemovePhoto(command.CatechistId, command.PhotoId, cancellationToken);
        await _storage.DeleteAsync(storageKey);
        return true;
    }
}