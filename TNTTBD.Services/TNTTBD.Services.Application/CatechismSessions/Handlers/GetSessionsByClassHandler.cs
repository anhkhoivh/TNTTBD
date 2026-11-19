using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.CatechismSessions.DTOs;
using TNTTBD.Services.Application.CatechismSessions.Interfaces;
using TNTTBD.Services.Application.CatechismSessions.Queries;
using TNTTBD.Services.Domain.Common;

namespace TNTTBD.Services.Application.CatechismSessions.Handlers;

public class GetSessionsByClassHandler : IRequestHandler<GetSessionsByClassQuery, List<CatechismSessionDto>>
{
    #region Fields

    private readonly ICatechismSessionQueryService _queryService;

    #endregion

    #region Constructors

    public GetSessionsByClassHandler(ICatechismSessionQueryService queryService)
    {
        _queryService = queryService;
    }

    #endregion

    #region Methods

    public async Task<List<CatechismSessionDto>> Handle(GetSessionsByClassQuery query, CancellationToken cancellationToken)
    {
        return await _queryService.GetSessionsByClassAsync(query.CatechismClassId, cancellationToken) ?? throw new DomainException(DomainErrors.ValidationNotFound);
    }

    #endregion
}