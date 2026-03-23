using MediatR;

namespace TNTTBD.Services.Application.Catechists.Commands;

public record RemoveCatechistCommand(
    Guid CatechistId
) : IRequest;