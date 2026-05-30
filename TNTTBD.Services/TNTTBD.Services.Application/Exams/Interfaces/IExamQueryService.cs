using TNTTBD.Services.Application.Common.DTOs;
using TNTTBD.Services.Application.Exams.DTOs;

namespace TNTTBD.Services.Application.Exams.Interfaces;

public interface IExamQueryService
{
    Task<ExamDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PaginatedResult<ExamDto>> GetAllAsync(Guid? classId, int page, int pageSize, CancellationToken cancellationToken);
    Task<PaginatedResult<MyExamResultDto>> GetMyResultsAsync(Guid youthId, int page, int pageSize, CancellationToken cancellationToken);
}
