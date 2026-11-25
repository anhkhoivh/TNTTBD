using TNTTBD.Services.Application.Abstractions;
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
            query.EnrollmentId,
            cancellationToken);
    }
}