using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.CatechismClassCatechists.Commands;

public record UpdateCatechistClassStatusCommand(
    Guid Id,
    EnrollmentStatus Status
);
