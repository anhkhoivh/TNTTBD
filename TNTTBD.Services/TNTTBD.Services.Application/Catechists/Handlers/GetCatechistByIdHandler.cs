using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Catechists.DTOs;
using TNTTBD.Services.Application.Catechists.Interfaces;
using TNTTBD.Services.Application.Catechists.Queries;
using TNTTBD.Services.Application.Common.Exceptions;
using TNTTBD.Services.Domain.Common;

namespace TNTTBD.Services.Application.Catechists.Handlers;

public class GetCatechistByIdHandler : IRequestHandler<GetCatechistByIdQuery, CatechistDto>
{
    #region Fields

    private readonly ICatechistQueryService _queryService;
    private readonly IFileStorage _storage;
    private readonly IAuthorizationContext _authContext;

    #endregion

    #region Constructors

    public GetCatechistByIdHandler(
        ICatechistQueryService queryService,
        IFileStorage fileStorage,
        IAuthorizationContext authContext)
    {
        _queryService = queryService;
        _storage = fileStorage;
        _authContext = authContext;
    }

    #endregion

    #region Methods

    public async Task<CatechistDto> Handle(GetCatechistByIdQuery query, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanViewCatechistAsync(query.Id, cancellationToken))
        {
            throw new ForbiddenException();
        }

        return await _queryService.GetByIdAsync(query.Id, _storage, cancellationToken) ?? throw new DomainException(DomainErrors.ValidationNotFound);
    }

    #endregion
}