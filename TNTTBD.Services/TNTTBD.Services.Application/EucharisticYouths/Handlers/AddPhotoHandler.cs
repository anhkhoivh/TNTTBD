using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.EucharisticYouths.Commands;
using TNTTBD.Services.Domain.Catechism.Members;

namespace TNTTBD.Services.Application.EucharisticYouths.Handlers;

/// <summary>
/// Add milestone handler
/// </summary>
public class AddPhotoHandler : IRequestHandler<AddPhotoCommand>
{
    #region Fields

    private readonly IEucharisticYouthRepository _repository;
    private readonly IUnitOfWork  _unitOfWork;

    #endregion

    #region Constructors

    public AddPhotoHandler(IEucharisticYouthRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    #endregion

    #region Methods

    public async Task Handle(AddPhotoCommand command, CancellationToken cancellationToken)
    {
        PhotoReference photoReference = new(command.StorageKey, command.ContentType, command.Purpose, command.UploadBy, command.Visibility);
        await _repository.AddPhoto(command.EucharisticYouthId, photoReference, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    #endregion
}