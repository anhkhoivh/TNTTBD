using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using TNTTBD.Services.Application.Auth.DTOs;
using TNTTBD.Services.Application.Auth.Interfaces;

namespace TNTTBD.Services.Infrastructure.Auth;

public class OAuthService : IOAuthService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public OAuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public string GetAuthorizationUrl(string provider, string redirectUri, string state)
    {
        return provider.ToLowerInvariant() switch
        {
            "google" => BuildGoogleAuthUrl(redirectUri, state),
            "microsoft" => BuildMicrosoftAuthUrl(redirectUri, state),
            _ => throw new ArgumentException($"Unsupported OAuth provider: {provider}")
        };
    }

    public async Task<OAuthUserInfo> ExchangeCodeAsync(string provider, string code, string redirectUri, CancellationToken cancellationToken)
    {
        return provider.ToLowerInvariant() switch
        {
            "google" => await ExchangeGoogleCodeAsync(code, redirectUri, cancellationToken),
            "microsoft" => await ExchangeMicrosoftCodeAsync(code, redirectUri, cancellationToken),
            _ => throw new ArgumentException($"Unsupported OAuth provider: {provider}")
        };
    }

    private string BuildGoogleAuthUrl(string redirectUri, string state)
    {
        var clientId = _configuration["OAuth:Google:ClientId"]
            ?? throw new InvalidOperationException("OAuth:Google:ClientId not configured.");
        var authEndpoint = _configuration["OAuth:Google:AuthorizationEndpoint"]!;

        var query = new Dictionary<string, string>
        {
            ["client_id"] = clientId,
            ["redirect_uri"] = redirectUri,
            ["response_type"] = "code",
            ["scope"] = "openid email profile",
            ["state"] = state,
            ["access_type"] = "offline",
            ["prompt"] = "select_account"
        };

        return $"{authEndpoint}?{BuildQueryString(query)}";
    }

    private string BuildMicrosoftAuthUrl(string redirectUri, string state)
    {
        var clientId = _configuration["OAuth:Microsoft:ClientId"]
            ?? throw new InvalidOperationException("OAuth:Microsoft:ClientId not configured.");
        var tenantId = _configuration["OAuth:Microsoft:TenantId"] ?? "common";
        var authEndpoint = _configuration["OAuth:Microsoft:AuthorizationEndpoint"]!
            .Replace("{TenantId}", tenantId);

        var query = new Dictionary<string, string>
        {
            ["client_id"] = clientId,
            ["redirect_uri"] = redirectUri,
            ["response_type"] = "code",
            ["scope"] = "openid email profile",
            ["state"] = state,
            ["response_mode"] = "query"
        };

        return $"{authEndpoint}?{BuildQueryString(query)}";
    }

    private async Task<OAuthUserInfo> ExchangeGoogleCodeAsync(string code, string redirectUri, CancellationToken cancellationToken)
    {
        var clientId = _configuration["OAuth:Google:ClientId"]!;
        var clientSecret = _configuration["OAuth:Google:ClientSecret"]!;
        var tokenEndpoint = _configuration["OAuth:Google:TokenEndpoint"]!;

        var tokenResponse = await ExchangeCodeForTokens(tokenEndpoint, new Dictionary<string, string>
        {
            ["code"] = code,
            ["client_id"] = clientId,
            ["client_secret"] = clientSecret,
            ["redirect_uri"] = redirectUri,
            ["grant_type"] = "authorization_code"
        }, cancellationToken);

        return DecodeIdToken(tokenResponse, "google");
    }

    private async Task<OAuthUserInfo> ExchangeMicrosoftCodeAsync(string code, string redirectUri, CancellationToken cancellationToken)
    {
        var clientId = _configuration["OAuth:Microsoft:ClientId"]!;
        var clientSecret = _configuration["OAuth:Microsoft:ClientSecret"]!;
        var tenantId = _configuration["OAuth:Microsoft:TenantId"] ?? "common";
        var tokenEndpoint = _configuration["OAuth:Microsoft:TokenEndpoint"]!
            .Replace("{TenantId}", tenantId);

        var tokenResponse = await ExchangeCodeForTokens(tokenEndpoint, new Dictionary<string, string>
        {
            ["code"] = code,
            ["client_id"] = clientId,
            ["client_secret"] = clientSecret,
            ["redirect_uri"] = redirectUri,
            ["grant_type"] = "authorization_code",
            ["scope"] = "openid email profile"
        }, cancellationToken);

        return DecodeIdToken(tokenResponse, "microsoft");
    }

    private async Task<OAuthTokenResponse> ExchangeCodeForTokens(
        string tokenEndpoint,
        Dictionary<string, string> parameters,
        CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient("OAuth");

        using var content = new FormUrlEncodedContent(parameters);
        var response = await client.PostAsync(tokenEndpoint, content, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new InvalidOperationException($"OAuth token exchange failed: {response.StatusCode} - {errorBody}");
        }

        var result = await response.Content.ReadFromJsonAsync<OAuthTokenResponse>(cancellationToken)
            ?? throw new InvalidOperationException("Failed to deserialize token response.");

        return result;
    }

    private static OAuthUserInfo DecodeIdToken(OAuthTokenResponse tokenResponse, string provider)
    {
        if (string.IsNullOrEmpty(tokenResponse.IdToken))
        {
            throw new InvalidOperationException("No id_token received from OAuth provider.");
        }

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(tokenResponse.IdToken);

        var sub = jwt.Claims.FirstOrDefault(c => c.Type == "sub")?.Value
            ?? throw new InvalidOperationException("Missing 'sub' claim in ID token.");

        var email = jwt.Claims.FirstOrDefault(c => c.Type == "email")?.Value
            ?? jwt.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value
            ?? throw new InvalidOperationException("Missing 'email' claim in ID token.");

        var name = jwt.Claims.FirstOrDefault(c => c.Type == "name")?.Value
            ?? jwt.Claims.FirstOrDefault(c => c.Type == "given_name")?.Value
            ?? email;

        return new OAuthUserInfo(email, sub, name, provider);
    }

    private static string BuildQueryString(Dictionary<string, string> parameters)
    {
        return string.Join("&", parameters.Select(p => $"{Uri.EscapeDataString(p.Key)}={Uri.EscapeDataString(p.Value)}"));
    }

    private sealed record OAuthTokenResponse
    {
        public string? IdToken { get; init; }
        public string? AccessToken { get; init; }
        public string? TokenType { get; init; }

        // Use JsonPropertyName for snake_case deserialization
        [System.Text.Json.Serialization.JsonPropertyName("id_token")]
        public string? IdTokenSnake { init { IdToken = value; } get { return IdToken; } }

        [System.Text.Json.Serialization.JsonPropertyName("access_token")]
        public string? AccessTokenSnake { init { AccessToken = value; } get { return AccessToken; } }

        [System.Text.Json.Serialization.JsonPropertyName("token_type")]
        public string? TokenTypeSnake { init { TokenType = value; } get { return TokenType; } }
    }
}
