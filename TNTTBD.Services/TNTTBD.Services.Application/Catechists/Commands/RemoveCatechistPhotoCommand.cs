using MediatR;

namespace TNTTBD.Services.Application.Catechists.Commands;

public record RemoveCatechistPhotoCommand(
    Guid CatechistId,
    Guid PhotoId
) : IRequest;