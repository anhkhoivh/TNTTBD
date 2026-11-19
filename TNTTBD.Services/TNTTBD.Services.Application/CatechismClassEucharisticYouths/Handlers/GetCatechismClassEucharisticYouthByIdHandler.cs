using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.DTOs;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.Interfaces;
using TNTTBD.Services.Application.CatechismClassEucharisticYouths.Queries;
using TNTTBD.Services.Domain.Common;

namespace TNTTBD.Services.Application.CatechismClassEucharisticYouths.Handlers;

public class GetCatechismClassEucharisticYouthByIdHandler : IRequestHandler<GetCatechismClassEucharisticYouthByIdQuery, CatechismClassEucharisticYouthDto>
{
    private readonly ICatechismClassEucharisticYouthQueryService _queryService;

    public GetCatechismClassEucharisticYouthByIdHandler(ICatechismClassEucharisticYouthQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<CatechismClassEucharisticYouthDto> Handle(GetCatechismClassEucharisticYouthByIdQuery query, CancellationToken cancellationToken)
    {
        return await _queryService.GetByIdAsync(query.Id, cancellationToken)
            ?? throw new DomainException(DomainErrors.ValidationNotFound);
    }
}
