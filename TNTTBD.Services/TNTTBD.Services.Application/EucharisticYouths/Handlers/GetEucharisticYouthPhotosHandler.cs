using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.EucharisticYouths.DTOs;
using TNTTBD.Services.Application.EucharisticYouths.Interfaces;
using TNTTBD.Services.Application.EucharisticYouths.Queries;
using TNTTBD.Services.Domain.Catechism.Members;
using TNTTBD.Services.Domain.Catechism.Repositories;
using TNTTBD.Services.Domain.Common;

namespace TNTTBD.Services.Application.EucharisticYouths.Handlers;

public class GetEucharisticYouthPhotosHandler : IRequestHandler<GetEucharisticYouthPhotosQuery, List<PhotoReferenceDto>>
{
    #region Fields

    private readonly IEucharisticYouthQueryService _queryService;
    private readonly IFileStorage _storage;

    #endregion

    #region Constructors

    public GetEucharisticYouthPhotosHandler(IEucharisticYouthQueryService queryService, IFileStorage fileStorage)
    {
        _queryService = queryService;
        _storage = fileStorage;
    }

    #endregion

    #region Methods

    public async Task<List<PhotoReferenceDto>> Handle(GetEucharisticYouthPhotosQuery query, CancellationToken cancellationToken)
    {
        EucharisticYouthDto? youth = await _queryService.GetByIdAsync(query.Id, _storage, cancellationToken);

        if (youth == null)
        {
            throw new DomainException(DomainErrors.ValidationNotFound);
        }

        return youth.Photos;
    }

    #endregion
}