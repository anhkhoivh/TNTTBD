using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.CatechismClassCatechists.Commands;

public record UpdateCatechistClassAssignmentCommand(
    Guid Id,
    CatechismClassCatechistRole Role,
    string? Note,
    DateTimeOffset? LeftAt
);
