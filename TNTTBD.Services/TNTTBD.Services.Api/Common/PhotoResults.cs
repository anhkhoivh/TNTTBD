using Microsoft.AspNetCore.Mvc;
using TNTTBD.Services.Application.Abstractions;
using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Api.Common;

public static class PhotoResults
{
    /// <summary>
    /// Stream a member's profile photo; 204 when there is none or its file is missing.
    /// </summary>
    public static IActionResult ProfilePhotoResult(this ControllerBase controller, ProfilePhotoFileDto? photo, IFileStorage storage)
    {
        Stream? stream = photo == null ? null : storage.OpenRead(photo.StorageKey);
        controller.Response.Headers.CacheControl = "private, no-cache";
        if (photo == null || stream == null)
        {
            return controller.NoContent();
        }

        return controller.File(stream, photo.ContentType);
    }
}
