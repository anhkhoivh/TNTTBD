using Microsoft.AspNetCore.Http;

namespace TNTTBD.Services.Infrastructure.Images;

public interface IImageProcessor
{
    Task<Stream> ProcessAsync(IFormFile file);
}