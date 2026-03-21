using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.EucharisticYouths.Queries;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Common;

namespace TNTTBD.Services.Application.EucharisticYouths.Handlers;

public class GetEucharisticYouthPhotosHandler : IRequestHandler<GetEucharisticYouthPhotosQuery, List<PhotoReferenceDto>>
{
    #region Fields

    private readonly IEucharisticYouthRepository _repository;
    private readonly IFileStorage _storage;

    #endregion

    #region Constructors

    public GetEucharisticYouthPhotosHandler(IEucharisticYouthRepository repository, IFileStorage fileStorage)
    {
        _repository = repository;
        _storage = fileStorage;
    }

    #endregion

    #region Methods

    public async Task<List<PhotoReferenceDto>> Handle(GetEucharisticYouthPhotosQuery query, CancellationToken cancellationToken)
    {
        EucharisticYouth? youth = await _repository.GetByIdAsync(query.Id, cancellationToken);

        if (youth == null)
        {
            throw new Exception(DomainErrors.ValidationNotFound);
        }

        return youth.Photos.Select(
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