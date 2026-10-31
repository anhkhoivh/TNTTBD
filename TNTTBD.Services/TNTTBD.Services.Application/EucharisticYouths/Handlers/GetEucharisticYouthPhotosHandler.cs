using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.Common.Exceptions;
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
    private readonly IAuthorizationContext _authContext;

    #endregion

    #region Constructors

    public GetEucharisticYouthPhotosHandler(
        IEucharisticYouthQueryService queryService,
        IFileStorage fileStorage,
        IAuthorizationContext authContext)
    {
        _queryService = queryService;
        _storage = fileStorage;
        _authContext = authContext;
    }

    #endregion

    #region Methods

    public async Task<List<PhotoReferenceDto>> Handle(GetEucharisticYouthPhotosQuery query, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanViewYouthAsync(query.Id, cancellationToken))
        {
            throw new ForbiddenException();
        }

        EucharisticYouthDto? youth = await _queryService.GetByIdAsync(query.Id, _storage, cancellationToken);

        if (youth == null)
        {
            throw new DomainException(DomainErrors.ValidationNotFound);
        }

        return youth.Photos;
    }

    #endregion
}