namespace TNTTBD.Services.Application.Users.Commands;

public record AdminResetPasswordCommand(
    Guid UserId,
    string NewPassword
);
