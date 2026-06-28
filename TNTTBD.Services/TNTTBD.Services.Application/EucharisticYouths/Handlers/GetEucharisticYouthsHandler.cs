using MediatR;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.EucharisticYouths.DTOs;
using TNTTBD.Services.Application.EucharisticYouths.Interfaces;
using TNTTBD.Services.Application.EucharisticYouths.Queries;

namespace TNTTBD.Services.Application.EucharisticYouths.Handlers;

public class GetEucharisticYouthsHandler : IRequestHandler<GetEucharisticYouthsQuery, PaginatedResult<EucharisticYouthDto>>
{
    private readonly IEucharisticYouthQueryService _queryService;
    private readonly IFileStorage _fileStorage;

    public GetEucharisticYouthsHandler(IEucharisticYouthQueryService queryService, IFileStorage fileStorage)
    {
        _queryService = queryService;
        _fileStorage = fileStorage;
    }

    public async Task<PaginatedResult<EucharisticYouthDto>> Handle(GetEucharisticYouthsQuery query, CancellationToken cancellationToken)
    {
        return await _queryService.GetAllAsync(query.Search, query.Page, query.PageSize, _fileStorage, cancellationToken);
    }
}
