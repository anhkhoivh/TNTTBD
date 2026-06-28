using MediatR;
using TNTTBD.Services.Application.CatechismClasses.DTOs;
using TNTTBD.Services.Application.CatechismClasses.Interfaces;
using TNTTBD.Services.Application.CatechismClasses.Queries;
using TNTTBD.Services.Domain.Common;

namespace TNTTBD.Services.Application.CatechismClasses.Handlers;

public class GetClassesByYearHandler : IRequestHandler<GetClassesByYearQuery, List<CatechismClassDto>>
{
    #region Fields

    private readonly ICatechismClassQueryService _queryService;

    #endregion

    #region Constructors

    public GetClassesByYearHandler(ICatechismClassQueryService queryService)
    {
        _queryService = queryService;
    }

    #endregion

    #region Methods

    public async Task<List<CatechismClassDto>> Handle(GetClassesByYearQuery query, CancellationToken cancellationToken)
    {
        return await _queryService.GetClassesByYearAsync(query.CatechismYearId, cancellationToken) ?? throw new DomainException(DomainErrors.ValidationNotFound);
    }

    #endregion
}