using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.Catechists.Commands;

public class RemoveCatechistPhotoHandler : IRequestHandler<RemoveCatechistPhotoCommand>
{
    private readonly ICatechistRepository _repository;
    private readonly IFileStorage _storage;

    public RemoveCatechistPhotoHandler(ICatechistRepository repository, IFileStorage storage)
    {
        _repository = repository;
        _storage = storage;
    }

    public async Task Handle(RemoveCatechistPhotoCommand command, CancellationToken cancellationToken)
    {
        string storageKey = await _repository.RemovePhoto(command.CatechistId, command.PhotoId, cancellationToken);
        await _storage.DeleteAsync(storageKey);
    }
}