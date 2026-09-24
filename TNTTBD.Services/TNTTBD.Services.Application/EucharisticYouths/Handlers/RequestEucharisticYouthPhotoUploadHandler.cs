using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public class RequestEucharisticYouthPhotoUploadHandler : IRequestHandler<RequestEucharisticYouthPhotoUploadCommand, PhotoReferenceDto?>
{
    private readonly IEucharisticYouthRepository _repository;
    private readonly IFileStorage _storage;
    private readonly IAuthorizationContext _authContext;

    public RequestEucharisticYouthPhotoUploadHandler(IEucharisticYouthRepository repository, IFileStorage fileStorage, IAuthorizationContext authContext)
    {
        _repository = repository;
        _storage = fileStorage;
        _authContext = authContext;
    }

    public async Task<PhotoReferenceDto?> Handle(RequestEucharisticYouthPhotoUploadCommand command, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanAccessYouthAsync(command.EucharisticYouthId, cancellationToken))
        {
            return null;
        }

        Guid photoId = Guid.NewGuid();

        string extension = Path.GetExtension(command.FileName);

        string storageKey = $"youths/{command.EucharisticYouthId}/photos/{photoId}{extension}";
        PhotoReference photoReference = new(photoId, storageKey, command.ContentType, command.Purpose, command.UploadedBy, DateTimeOffset.Now.ToUniversalTime(), command.Visibility);
        await _repository.AddPhoto(command.EucharisticYouthId, photoReference, cancellationToken);
        string uploadUrl = await _storage.GenerateUploadUrl(storageKey, command.ContentType, TimeSpan.FromMinutes(5));

        return new PhotoReferenceDto
        {
            PhotoId = photoId,
            Url = uploadUrl,
            ContentType = command.ContentType,
            Purpose = command.Purpose.ToString(),
            UploadedBy = command.UploadedBy
        };
    }
}