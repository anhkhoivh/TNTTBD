using MediatR;

namespace TNTTBD.Services.Application.Catechists.Commands;

public record AddCatechistPatronSaintCommand(
    Guid CatechistId,
    string SaintName,
    int Day,
    int Month
) : IRequest;