using MediatR;
using TNTTBD.Services.Domain.Enums;

namespace TNTTBD.Services.Application.CatechismClassCatechists.Commands;

public record AssignCatechistToClassCommand(
    Guid CatechismClassId,
    Guid CatechistId,
    CatechismClassCatechistRole Role,
    string? Note
) : IRequest<Guid?>;
