using System.Globalization;
using System.Text.Json;

namespace TNTTBD.Services.Api.Common.Localization;

public class JsonLocalizationService : ILocalizationService
{
    private readonly IWebHostEnvironment _env;
    private readonly Dictionary<string, Dictionary<string, string>> _cache;

    public JsonLocalizationService(IWebHostEnvironment env)
    {
        _env = env;
        _cache = LoadResources();
    }

    private Dictionary<string, Dictionary<string, string>> LoadResources()
    {
        var resourcesPath = Path.Combine(_env.ContentRootPath, "Resources");

        return Directory.GetFiles(resourcesPath, "*.json")
            .ToDictionary(
                file => Path.GetFileNameWithoutExtension(file),
                file => JsonSerializer.Deserialize<Dictionary<string, string>>(
                            File.ReadAllText(file))!
            );
    }

    public string Get(string key)
    {
        var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

        if (_cache.TryGetValue(culture, out var dict) &&
            dict.TryGetValue(key, out var value))
        {
            return value;
        }

        // fallback to English
        if (_cache.TryGetValue("vi", out var enDict) &&
            enDict.TryGetValue(key, out var enValue))
        {
            return enValue;
        }

        return key; // fallback ultimate
    }

    public string Get(string key, params object[] args)
    {
        var format = Get(key);
        return string.Format(format, args);
    }
}