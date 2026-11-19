using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Auth.Interfaces;
using TNTTBD.Services.Application.Catechists.DTOs;
using TNTTBD.Services.Application.Catechists.Interfaces;
using TNTTBD.Services.Application.Catechists.Queries;

namespace TNTTBD.Services.Application.Catechists.Handlers;

public class GetCatechistCurrentClassHandler : IRequestHandler<GetCatechistCurrentClassQuery, CatechistCurrentClassDto?>
{
    private readonly ICatechistQueryService _queryService;
    private readonly IAuthorizationContext _authContext;

    public GetCatechistCurrentClassHandler(
        ICatechistQueryService queryService,
        IAuthorizationContext authContext)
    {
        _queryService = queryService;
        _authContext = authContext;
    }

    public async Task<CatechistCurrentClassDto?> Handle(GetCatechistCurrentClassQuery query, CancellationToken cancellationToken)
    {
        Guid? catechistId = await _authContext.GetCurrentCatechistIdAsync(cancellationToken);
        return catechistId.HasValue
            ? await _queryService.GetCurrentClassAsync(catechistId.Value, cancellationToken)
            : null;
    }
}
