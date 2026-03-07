using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public class RemoveYouthPhotoHandler
    : IRequestHandler<RemoveYouthPhotoCommand>
{
    private readonly IEucharisticYouthRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileStorage _storage;

    public RemoveYouthPhotoHandler(IEucharisticYouthRepository repository, IUnitOfWork unitOfWork, IFileStorage storage)
    {
        _repository = repository;
        _storage = storage;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RemoveYouthPhotoCommand command, CancellationToken cancellationToken)
    {
        string storageKey = await _repository.RemovePhoto(command.EucharisticYouthId, command.PhotoId, cancellationToken);
        await _storage.DeleteAsync(storageKey);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}