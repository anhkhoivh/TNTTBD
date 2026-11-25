namespace TNTTBD.Services.Application.EucharisticYouths.Commands;

/// <summary>
/// Returns the removed file's storage key, or null when forbidden.
/// </summary>
public record RemoveEucharisticYouthPhotoCommand(
    Guid EucharisticYouthId,
    Guid PhotoId
);