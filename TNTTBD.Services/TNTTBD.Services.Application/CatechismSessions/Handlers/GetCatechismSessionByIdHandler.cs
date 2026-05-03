using MediatR;
using TNTTBD.Services.Application.CatechismSessions.DTOs;
using TNTTBD.Services.Application.CatechismSessions.Interfaces;
using TNTTBD.Services.Application.CatechismSessions.Queries;
using TNTTBD.Services.Domain.Common;

namespace TNTTBD.Services.Application.CatechismSessions.Handlers;

public class GetCatechismSessionByIdHandler : IRequestHandler<GetCatechismSessionByIdQuery, CatechismSessionDto>
{
    #region Fields

    private readonly ICatechismSessionQueryService _queryService;

    #endregion

    #region Constructors

    public GetCatechismSessionByIdHandler(ICatechismSessionQueryService queryService)
    {
        _queryService = queryService;
    }

    #endregion

    #region Methods

    public async Task<CatechismSessionDto> Handle(GetCatechismSessionByIdQuery query, CancellationToken cancellationToken)
    {
        return await _queryService.GetByIdAsync(query.Id, cancellationToken) ?? throw new Exception(DomainErrors.ValidationNotFound);
    }

    #endregion
}