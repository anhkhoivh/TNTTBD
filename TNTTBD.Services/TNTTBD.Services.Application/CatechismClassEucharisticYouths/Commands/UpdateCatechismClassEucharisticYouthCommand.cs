using MediatR;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.CatechismClassEucharisticYouths.Commands;

public record UpdateCatechismClassEucharisticYouthCommand(
    Guid Id,
    EnrollmentStatus Status,
    string Note
) : IRequest<bool>;