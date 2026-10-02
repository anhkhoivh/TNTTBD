using MediatR;
using TNTTBD.Services.Application.Reports.DTOs;

namespace TNTTBD.Services.Application.Reports.Queries;

public record GetClassReportQuery(Guid ClassId) : IRequest<ClassReportDto?>;