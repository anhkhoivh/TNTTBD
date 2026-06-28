using MediatR;
using TNTTBD.Services.Application.CatechismYears.DTOs;
using TNTTBD.Services.Application.CatechismYears.Interfaces;
using TNTTBD.Services.Application.CatechismYears.Queries;
using TNTTBD.Services.Domain.Common;

namespace TNTTBD.Services.Application.CatechismYears.Handlers;

public class GetCatechismYearByIdHandler : IRequestHandler<GetCatechismYearByIdQuery, CatechismYearDto>
{
    #region Fields

    private readonly ICatechismYearQueryService _queryService;

    #endregion

    #region Constructors

    public GetCatechismYearByIdHandler(ICatechismYearQueryService queryService)
    {
        _queryService = queryService;
    }

    #endregion

    #region Methods

    public async Task<CatechismYearDto> Handle(GetCatechismYearByIdQuery query, CancellationToken cancellationToken)
    {
        return await _queryService.GetByIdAsync(query.Id, cancellationToken) ?? throw new DomainException(DomainErrors.ValidationNotFound);
    }

    #endregion
}