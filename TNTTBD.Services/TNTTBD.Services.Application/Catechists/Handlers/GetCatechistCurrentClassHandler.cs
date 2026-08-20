using MediatR;
using TNTTBD.Services.Application.Catechists.DTOs;
using TNTTBD.Services.Application.Catechists.Interfaces;
using TNTTBD.Services.Application.Catechists.Queries;

namespace TNTTBD.Services.Application.Catechists.Handlers;

public class GetCatechistCurrentClassHandler : IRequestHandler<GetCatechistCurrentClassQuery, CatechistCurrentClassDto?>
{
    private readonly ICatechistQueryService _queryService;

    public GetCatechistCurrentClassHandler(ICatechistQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<CatechistCurrentClassDto?> Handle(GetCatechistCurrentClassQuery query, CancellationToken cancellationToken)
    {
        return await _queryService.GetCurrentClassAsync(query.CatechistId, cancellationToken);
    }
}
