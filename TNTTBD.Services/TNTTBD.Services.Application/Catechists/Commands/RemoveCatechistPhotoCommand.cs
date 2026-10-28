using MediatR;

namespace TNTTBD.Services.Application.Catechists.Commands;

/// <summary>
/// Returns the removed file's storage key, or null when forbidden.
/// </summary>
public record RemoveCatechistPhotoCommand(
    Guid CatechistId,
    Guid PhotoId
) : IRequest<string?>;