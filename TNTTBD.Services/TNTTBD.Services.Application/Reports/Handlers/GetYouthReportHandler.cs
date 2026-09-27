using MediatR;
using TNTTBD.Services.Application.Reports.DTOs;
using TNTTBD.Services.Application.Reports.Interfaces;
using TNTTBD.Services.Application.Reports.Queries;
using TNTTBD.Services.Application.Auth.Interfaces;

namespace TNTTBD.Services.Application.Reports.Handlers;

public class GetYouthReportHandler(IReportQueryService queryService, IAuthorizationContext authorizationContext)
    : IRequestHandler<GetYouthReportQuery, YouthReportDto?>
{
    public async Task<YouthReportDto?> Handle(GetYouthReportQuery query, CancellationToken cancellationToken)
    {
        if (!await authorizationContext.CanReadYouthContactsAsync(query.YouthId, cancellationToken))
        {
            return null;
        }

        return await queryService.GetYouthReportAsync(
            query.YouthId,
            query.CatechismYearId,
            query.Page,
            query.PageSize,
            cancellationToken);
    }
}

public class GetClassReportHandler(IReportQueryService queryService, IAuthorizationContext authorizationContext)
    : IRequestHandler<GetClassReportQuery, ClassReportDto?>
{
    public async Task<ClassReportDto?> Handle(GetClassReportQuery query, CancellationToken cancellationToken)
    {
        if (!await authorizationContext.IsAdminOrCatechistAsync(cancellationToken)
            || !await authorizationContext.CanAccessClassAsync(query.ClassId, cancellationToken))
        {
            return null;
        }

        return await queryService.GetClassReportAsync(query.ClassId, cancellationToken);
    }
}