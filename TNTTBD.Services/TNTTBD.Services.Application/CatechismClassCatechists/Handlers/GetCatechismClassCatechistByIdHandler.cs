using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.CatechismClassCatechists.DTOs;
using TNTTBD.Services.Application.CatechismClassCatechists.Interfaces;
using TNTTBD.Services.Application.CatechismClassCatechists.Queries;

namespace TNTTBD.Services.Application.CatechismClassCatechists.Handlers;

public class GetCatechismClassCatechistByIdHandler : IRequestHandler<GetCatechismClassCatechistByIdQuery, CatechismClassCatechistDto>
{
    private readonly ICatechismClassCatechistQueryService _queryService;

    public GetCatechismClassCatechistByIdHandler(ICatechismClassCatechistQueryService queryService)
    {
        _queryService = queryService;
    }

    public async Task<CatechismClassCatechistDto> Handle(GetCatechismClassCatechistByIdQuery query, CancellationToken cancellationToken)
    {
        return await _queryService.GetByIdAsync(query.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"CatechismClassCatechist with id {query.Id} not found.");
    }
}
