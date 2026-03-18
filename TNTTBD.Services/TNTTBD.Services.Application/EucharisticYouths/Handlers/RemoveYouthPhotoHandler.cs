using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public class RemoveYouthPhotoHandler
    : IRequestHandler<RemoveYouthPhotoCommand>
{
    private readonly IEucharisticYouthRepository _repository;
    private readonly IFileStorage _storage;

    public RemoveYouthPhotoHandler(IEucharisticYouthRepository repository, IFileStorage storage)
    {
        _repository = repository;
        _storage = storage;
    }

    public async Task Handle(RemoveYouthPhotoCommand command, CancellationToken cancellationToken)
    {
        string storageKey = await _repository.RemovePhoto(command.EucharisticYouthId, command.PhotoId, cancellationToken);
        await _storage.DeleteAsync(storageKey);
    }
}