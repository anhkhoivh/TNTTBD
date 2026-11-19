namespace TNTTBD.Services.Application.Auth.Commands;

public record SsoCallbackCommand(string Provider, string Code, string RedirectUri);
