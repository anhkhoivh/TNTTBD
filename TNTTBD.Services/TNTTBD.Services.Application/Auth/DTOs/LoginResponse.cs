namespace TNTTBD.Services.Application.Auth.DTOs;

public class LoginResponse
{
    public required string Token { get; set; }
    public required string RefreshToken { get; set; }
    public required UserDto User { get; set; }
}
