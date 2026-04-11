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

public class GetEucharisticYouthByIdHandler : IRequestHandler<GetEucharisticYouthByIdQuery, EucharisticYouthDto?>
{
    #region Fields
    
    private readonly IEucharisticYouthQueryService _queryService;
    private readonly IFileStorage _storage;

    #endregion

    #region Constructors

    public GetEucharisticYouthByIdHandler(IEucharisticYouthQueryService queryService, IFileStorage fileStorage)
    {
        _storage = fileStorage;
        _queryService = queryService;
    }

    #endregion

    #region Methods

    public async Task<EucharisticYouthDto?> Handle(GetEucharisticYouthByIdQuery query, CancellationToken cancellationToken)
    {
        EucharisticYouthDto? youthDto = await _queryService.GetByIdAsync(query.Id, _storage, cancellationToken);

        if (youthDto == null)
        {
            throw new Exception(DomainErrors.ValidationNotFound);
        }

        return youthDto;
    }

    #endregion
}