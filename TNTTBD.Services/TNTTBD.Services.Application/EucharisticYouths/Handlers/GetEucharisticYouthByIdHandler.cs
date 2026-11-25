using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Common.Exceptions;
using TNTTBD.Services.Application.EucharisticYouths.DTOs;
using TNTTBD.Services.Application.EucharisticYouths.Interfaces;
using TNTTBD.Services.Application.EucharisticYouths.Queries;
using TNTTBD.Services.Domain.Common;

namespace TNTTBD.Services.Application.EucharisticYouths.Handlers;

public class GetEucharisticYouthByIdHandler : IRequestHandler<GetEucharisticYouthByIdQuery, EucharisticYouthDto>
{
    #region Fields
    
    private readonly IEucharisticYouthQueryService _queryService;
    private readonly IFileStorage _storage;
    private readonly IAuthorizationContext _authContext;

    #endregion

    #region Constructors

    public GetEucharisticYouthByIdHandler(
        IEucharisticYouthQueryService queryService,
        IFileStorage fileStorage,
        IAuthorizationContext authContext)
    {
        _storage = fileStorage;
        _queryService = queryService;
        _authContext = authContext;
    }

    #endregion

    #region Methods

    public async Task<EucharisticYouthDto> Handle(GetEucharisticYouthByIdQuery query, CancellationToken cancellationToken)
    {
        if (!await _authContext.CanViewYouthAsync(query.Id, cancellationToken))
        {
            throw new ForbiddenException();
        }

        EucharisticYouthDto youth = await _queryService.GetByIdAsync(query.Id, _storage, cancellationToken)
            ?? throw new DomainException(DomainErrors.ValidationNotFound);

        // Parents / youths get a limited view: no godparent phone number
        if (!await _authContext.IsAdminOrCatechistAsync(cancellationToken))
        {
            youth.Milestones = youth.Milestones.Select(m => m with { SponsorPhoneNumber = null }).ToList();
        }

        return youth;
    }

    #endregion
}