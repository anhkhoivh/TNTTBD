using TNTTBD.Services.Application.Common.DTOs;

namespace TNTTBD.Services.Application.Abstractions;
public interface IFileStorage
{
    Task<string> GenerateUploadUrl(string storageKey, string contentType, TimeSpan expiry);

    string GetFileUrl(string storageKey);

    /// <summary>
    /// Open a stored file for reading; null when it does not exist.
    /// </summary>
    Stream? OpenRead(string storageKey);

    Task DeleteAsync(string storageKey);
}