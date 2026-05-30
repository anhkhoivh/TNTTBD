using MediatR;

namespace TNTTBD.Services.Application.CatechismClassCatechists.Commands;

public record RemoveCatechistFromClassCommand(Guid Id) : IRequest;
