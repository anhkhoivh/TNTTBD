using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

public class RequestEucharisticYouthPhotoUploadHandler : IRequestHandler<RequestEucharisticYouthPhotoUploadCommand, PhotoReferenceDto>
{
    private readonly IEucharisticYouthRepository _repository;
    private readonly IFileStorage _storage;
    private readonly IUserRepository _userRepository;

    public RequestEucharisticYouthPhotoUploadHandler(IEucharisticYouthRepository repository, IFileStorage fileStorage, IUserRepository userRepository)
    {
        _repository = repository;
        _storage = fileStorage;
        _userRepository = userRepository;
    }

    public async Task<PhotoReferenceDto> Handle(RequestEucharisticYouthPhotoUploadCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(command.UploadedBy, cancellationToken)
            ?? throw new UnauthorizedAccessException("User not found.");

        if (user.UserRole != UserRole.Admin && user.EucharisticYouthId != command.EucharisticYouthId)
            throw new UnauthorizedAccessException("You do not have permission to upload photos for this youth.");

        Guid photoId = Guid.NewGuid();

        string extension = Path.GetExtension(command.FileName);

        string storageKey = $"youths/{command.EucharisticYouthId}/photos/{photoId}{extension}";
        PhotoReference photoReference = new(photoId, storageKey, command.ContentType, command.Purpose, command.UploadedBy, DateTimeOffset.Now, command.Visibility);
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