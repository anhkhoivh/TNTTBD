using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.CatechismClasses.DTOs;
using TNTTBD.Services.Application.CatechismClasses.Interfaces;
using TNTTBD.Services.Application.CatechismClasses.Queries;
using TNTTBD.Services.Domain.Common;

namespace TNTTBD.Services.Application.CatechismClasses.Handlers;

public class GetCatechismClassByIdHandler : IRequestHandler<GetCatechismClassByIdQuery, CatechismClassDto>
{
    #region Fields

    private readonly ICatechismClassQueryService _queryService;

    #endregion

    #region Constructors

    public GetCatechismClassByIdHandler(ICatechismClassQueryService queryService)
    {
        _queryService = queryService;
    }

    #endregion

    #region Methods

    public async Task<CatechismClassDto> Handle(GetCatechismClassByIdQuery query, CancellationToken cancellationToken)
    {
        return await _queryService.GetByIdAsync(query.Id, cancellationToken) ?? throw new DomainException(DomainErrors.ValidationNotFound);
    }

    #endregion
}