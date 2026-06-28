using TNTTBD.Services.Application.Abstractions;

namespace TNTTBD.Services.Infrastructure.Storage;

public class LocalFileStorage : IFileStorage
{
    private readonly string _basePath;
    private readonly string _baseUrl;

    public LocalFileStorage(string basePath, string baseUrl)
    {
        _basePath = basePath;
        _baseUrl = baseUrl.TrimEnd('/');
    }

    public Task<string> GenerateUploadUrl(string storageKey, string contentType, TimeSpan expiry)
    {
        // For local storage, return direct upload endpoint
        string uploadUrl = $"{_baseUrl}/api/files/upload/{storageKey}";
        return Task.FromResult(uploadUrl);
    }

    public string GetFileUrl(string storageKey)
    {
        return $"{_baseUrl}/api/files/{storageKey}";
    }

    public Task DeleteAsync(string storageKey)
    {
        var filePath = Path.Combine(_basePath, storageKey.Replace('/', Path.DirectorySeparatorChar));
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        return Task.CompletedTask;
    }
}