using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Catechists.DTOs;
using TNTTBD.Services.Application.Catechists.Interfaces;
using TNTTBD.Services.Application.Catechists.Queries;
using TNTTBD.Services.Domain.Common;

namespace TNTTBD.Services.Application.Catechists.Handlers;

public class GetCatechistByIdHandler : IRequestHandler<GetCatechistByIdQuery, CatechistDto>
{
    #region Fields

    private readonly ICatechistQueryService _queryService;
    private readonly IFileStorage _storage;

    #endregion

    #region Constructors

    public GetCatechistByIdHandler(ICatechistQueryService queryService, IFileStorage fileStorage)
    {
        _queryService = queryService;
        _storage = fileStorage;
    }

    #endregion

    #region Methods

    public async Task<CatechistDto> Handle(GetCatechistByIdQuery query, CancellationToken cancellationToken)
    {
        return await _queryService.GetByIdAsync(query.Id, _storage, cancellationToken) ?? throw new DomainException(DomainErrors.ValidationNotFound);
    }

    #endregion
}