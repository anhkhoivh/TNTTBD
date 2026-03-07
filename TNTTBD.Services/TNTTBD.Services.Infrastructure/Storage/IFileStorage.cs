namespace TNTTBD.Services.Infrastructure.Storage;

public interface IFileStorage
{
    Task<string> UploadAsync(string path, Stream stream);

    Task DeleteAsync(string path);
}