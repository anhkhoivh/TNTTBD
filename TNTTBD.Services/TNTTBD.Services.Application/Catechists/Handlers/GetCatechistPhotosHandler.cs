using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Catechists.Interfaces;
using TNTTBD.Services.Application.Catechists.Queries;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Domain.Common;

namespace TNTTBD.Services.Application.Catechists.Handlers;

public class GetCatechistPhotosHandler : IRequestHandler<GetCatechistPhotosQuery, List<PhotoReferenceDto>>
{
    #region Fields

    private readonly ICatechistQueryService _queryService;
    private readonly IFileStorage _storage;

    #endregion

    #region Constructors

    public GetCatechistPhotosHandler(ICatechistQueryService queryService, IFileStorage fileStorage)
    {
        _queryService = queryService;
        _storage = fileStorage;
    }

    #endregion

    #region Methods

    public async Task<List<PhotoReferenceDto>> Handle(GetCatechistPhotosQuery query, CancellationToken cancellationToken)
    {
        return await _queryService.GetPhotosByCatechistId(query.CatechistId, _storage, cancellationToken) ?? throw new DomainException(DomainErrors.ValidationNotFound);
    }

    #endregion
}