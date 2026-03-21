using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Catechists.Queries;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Common;

namespace TNTTBD.Services.Application.Catechists.Handlers;

public class GetCatechistPhotosHandler : IRequestHandler<GetCatechistPhotosQuery, List<PhotoReferenceDto>>
{
    #region Fields

    private readonly ICatechistRepository _repository;
    private readonly IFileStorage _storage;

    #endregion

    #region Constructors

    public GetCatechistPhotosHandler(ICatechistRepository repository, IFileStorage fileStorage)
    {
        _repository = repository;
        _storage = fileStorage;
    }

    #endregion

    #region Methods

    public async Task<List<PhotoReferenceDto>> Handle(GetCatechistPhotosQuery query, CancellationToken cancellationToken)
    {
        Catechist? catechist = await _repository.GetByIdAsync(query.Id, cancellationToken);

        if (catechist == null)
        {
            throw new Exception(DomainErrors.ValidationNotFound);
        }

        return catechist.Photos.Select(
            m => new PhotoReferenceDto
                {
                    Purpose = m.Purpose.ToString(),
                    ContentType = m.ContentType,
                    UploadedBy = m.UploadedBy,
                    Url = _storage.GetFileUrl(m.StorageKey)
                }).ToList();
    }

    #endregion
}