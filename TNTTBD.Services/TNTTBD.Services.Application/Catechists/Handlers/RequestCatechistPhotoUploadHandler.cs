using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Catechism.Repositories;

namespace TNTTBD.Services.Application.Catechists.Commands;

public class RequestCatechistPhotoUploadHandler : IRequestHandler<RequestCatechistPhotoUploadCommand, PhotoReferenceDto>
{
    private readonly ICatechistRepository _repository;
    private readonly IFileStorage _storage;

    #region Constructors

    /// <summary>
    /// 
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="unitOfWork"></param>
    public RequestCatechistPhotoUploadHandler(ICatechistRepository repository, IFileStorage fileStorage)
    {
        _repository = repository;
        _storage = fileStorage;
    }

    #endregion

    public async Task<PhotoReferenceDto> Handle(RequestCatechistPhotoUploadCommand command, CancellationToken cancellationToken)
    {
        Guid photoId = Guid.NewGuid();

        string extension = Path.GetExtension(command.FileName);

        string storageKey = $"catechists/{command.CatechistId}/photos/{photoId}{extension}";
        PhotoReference photoReference = new(photoId, storageKey, command.ContentType, command.Purpose, command.UploadedBy, DateTimeOffset.Now, command.Visibility);
        await _repository.AddPhoto(command.CatechistId, photoReference, cancellationToken);
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