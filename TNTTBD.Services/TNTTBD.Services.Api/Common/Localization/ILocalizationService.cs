namespace TNTTBD.Services.Api.Common.Localization;

/// <summary>
/// Localization service 
/// </summary>
public interface ILocalizationService
{
    string Get(string key);
    string Get(string key, params object[] arguments);
}