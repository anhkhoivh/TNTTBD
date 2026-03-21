using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public class RemoveEucharisticYouthPhotoHandler : IRequestHandler<RemoveEucharisticYouthPhotoCommand>
{
    private readonly IEucharisticYouthRepository _repository;
    private readonly IFileStorage _storage;

    public RemoveEucharisticYouthPhotoHandler(IEucharisticYouthRepository repository, IFileStorage storage)
    {
        _repository = repository;
        _storage = storage;
    }

    public async Task Handle(RemoveEucharisticYouthPhotoCommand command, CancellationToken cancellationToken)
    {
        string storageKey = await _repository.RemovePhoto(command.EucharisticYouthId, command.PhotoId, cancellationToken);
        await _storage.DeleteAsync(storageKey);
    }
}