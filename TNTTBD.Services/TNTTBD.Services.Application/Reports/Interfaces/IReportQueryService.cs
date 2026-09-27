using TNTTBD.Services.Application.Reports.DTOs;

namespace TNTTBD.Services.Application.Reports.Interfaces;

public interface IReportQueryService
{
    Task<ClassReportDto?> GetClassReportAsync(Guid classId, CancellationToken cancellationToken);

    Task<YouthReportDto?> GetYouthReportAsync(
        Guid youthId,
        Guid? catechismYearId,
        int page,
        int? pageSize,
        CancellationToken cancellationToken);
}