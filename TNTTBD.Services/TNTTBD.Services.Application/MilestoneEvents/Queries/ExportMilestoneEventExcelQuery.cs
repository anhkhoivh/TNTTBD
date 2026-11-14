using MediatR;
using TNTTBD.Services.Application.MilestoneEvents.DTOs;

namespace TNTTBD.Services.Application.MilestoneEvents.Queries;

public record ExportMilestoneEventExcelQuery(Guid Id, string Language) : IRequest<MilestoneEventExcelFileDto?>;
